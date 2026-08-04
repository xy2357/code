const FACE_PIPS = {
  1: ["mc"],
  2: ["tl", "br"],
  3: ["tl", "mc", "br"],
  4: ["tl", "tr", "bl", "br"],
  5: ["tl", "tr", "mc", "bl", "br"],
  6: ["tl", "tr", "ml", "mr", "bl", "br"],
};

// These rotations are the exact inverse of each physical cube face transform.
// The value shown at the end of the roll therefore always matches the result used by game logic.
const FACE_ROTATIONS = {
  1: [0, 0],
  2: [-90, 0],
  3: [0, -90],
  4: [0, 90],
  5: [90, 0],
  6: [0, 180],
};

const $ = (selector) => document.querySelector(selector);
const wait = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

const ui = {
  shell: $(".game-shell"),
  rollButton: $("#rollButton"),
  holdButton: $("#holdButton"),
  soundButton: $("#soundButton"),
  helpButton: $("#helpButton"),
  rulesDialog: $("#rulesDialog"),
  closeRulesButton: $("#closeRulesButton"),
  understoodButton: $("#understoodButton"),
  messageBubble: $("#messageBubble"),
  messageKicker: $("#messageKicker"),
  messageText: $("#messageText"),
  playerScore: $("#playerScore"),
  opponentScore: $("#opponentScore"),
  statusChip: $("#statusChip"),
  playerGlow: $("#playerGlow"),
  opponentGlow: $("#opponentGlow"),
  history: $("#history"),
  tipText: $("#tipText"),
  resultOverlay: $("#resultOverlay"),
  resultCard: $(".result-card"),
  resultBadge: $("#resultBadge"),
  resultKicker: $("#resultKicker"),
  resultTitle: $("#resultTitle"),
  resultDescription: $("#resultDescription"),
  resultPlayerScore: $("#resultPlayerScore"),
  resultOpponentScore: $("#resultOpponentScore"),
  restartButton: $("#restartButton"),
};

const state = {
  playerScore: 0,
  opponentScore: 0,
  playerHasRolled: false,
  turn: "player",
  busy: false,
  gameOver: false,
  soundOn: true,
  audioContext: null,
  rollNumber: 0,
};

function createDie(host, initialValue = 1) {
  const cube = document.createElement("div");
  cube.className = "die-cube";

  for (let faceValue = 1; faceValue <= 6; faceValue += 1) {
    const face = document.createElement("div");
    face.className = "die-face";
    face.dataset.face = String(faceValue);
    face.setAttribute("aria-hidden", "true");
    FACE_PIPS[faceValue].forEach((position) => {
      const pip = document.createElement("i");
      pip.className = `pip pip--${position}`;
      face.appendChild(pip);
    });
    cube.appendChild(face);
  }

  host.appendChild(cube);
  host.dataset.value = String(initialValue);
  setCubeRotation(cube, initialValue, 0, true);
  return { host, cube, value: initialValue };
}

function setCubeRotation(cube, value, spinIndex = 0, instant = false) {
  const [baseX, baseY] = FACE_ROTATIONS[value];
  const xTurns = spinIndex % 2 === 0 ? 720 : -720;
  const yTurns = spinIndex % 3 === 0 ? 1080 : -1080;
  cube.style.transition = instant ? "none" : "";
  cube.style.transform = `rotateX(${baseX + (instant ? 0 : xTurns)}deg) rotateY(${baseY + (instant ? 0 : yTurns)}deg)`;
}

const dice = {
  player: [createDie($("#playerDie1"), 3), createDie($("#playerDie2"), 5)],
  opponent: [createDie($("#opponentDie1"), 2), createDie($("#opponentDie2"), 4)],
};

function randomDie() {
  return Math.floor(Math.random() * 6) + 1;
}

function ensureAudio() {
  if (!state.soundOn) return null;
  if (!state.audioContext) {
    const AudioContext = window.AudioContext || window.webkitAudioContext;
    if (!AudioContext) return null;
    state.audioContext = new AudioContext();
  }
  if (state.audioContext.state === "suspended") state.audioContext.resume();
  return state.audioContext;
}

function tone(frequency, duration, type = "sine", volume = 0.045, delay = 0) {
  const context = ensureAudio();
  if (!context) return;
  const oscillator = context.createOscillator();
  const gain = context.createGain();
  const start = context.currentTime + delay;
  oscillator.type = type;
  oscillator.frequency.setValueAtTime(frequency, start);
  oscillator.frequency.exponentialRampToValueAtTime(Math.max(55, frequency * .72), start + duration);
  gain.gain.setValueAtTime(volume, start);
  gain.gain.exponentialRampToValueAtTime(.001, start + duration);
  oscillator.connect(gain).connect(context.destination);
  oscillator.start(start);
  oscillator.stop(start + duration);
}

function playRollSound() {
  [0, .11, .23, .38, .62, .82].forEach((delay, index) => {
    tone(155 + (index % 3) * 46, .08, "square", .022, delay);
  });
  tone(95, .18, "triangle", .055, .9);
  tone(130, .12, "triangle", .04, .98);
}

function playScoreSound() {
  tone(390, .12, "triangle", .035);
  tone(520, .18, "triangle", .032, .08);
}

function playResultSound(won) {
  const notes = won ? [523, 659, 784, 1046] : [330, 277, 220];
  notes.forEach((note, index) => tone(note, .3, "triangle", .045, index * .12));
}

async function animateDice(owner, values) {
  state.rollNumber += 1;
  const pair = dice[owner];
  playRollSound();

  pair.forEach((die, index) => {
    die.host.classList.remove("is-rolling");
    void die.host.offsetWidth;
    die.host.classList.add("is-rolling");

    // Start from a clean readable orientation, then rotate several full turns to the exact target face.
    setCubeRotation(die.cube, die.value, 0, true);
    void die.cube.offsetWidth;
    setCubeRotation(die.cube, values[index], state.rollNumber + index, false);
  });

  await wait(1160);

  pair.forEach((die, index) => {
    die.host.classList.remove("is-rolling");
    die.value = values[index];
    die.host.dataset.value = String(values[index]);
    die.host.setAttribute("aria-label", `${values[index]} 点`);
    setCubeRotation(die.cube, values[index], 0, true);
  });
}

function setMessage(kicker, text, owner = "player") {
  ui.messageKicker.textContent = kicker;
  ui.messageText.textContent = text;
  ui.messageBubble.classList.toggle("is-bot", owner === "opponent");
  ui.messageBubble.style.animation = "none";
  void ui.messageBubble.offsetWidth;
  ui.messageBubble.style.animation = "";
}

function popScore(element, score) {
  element.textContent = String(score);
  element.classList.remove("score-pop");
  void element.offsetWidth;
  element.classList.add("score-pop");
  element.classList.toggle("danger", score >= 18 && score <= 21);
}

function addHistory(owner, values) {
  const empty = ui.history.querySelector(".history-empty");
  if (empty) empty.remove();
  const item = document.createElement("span");
  item.className = "history-roll";
  item.dataset.owner = owner;
  item.textContent = `${owner === "player" ? "你" : "机"} ${values[0]}+${values[1]}`;
  ui.history.appendChild(item);
  while (ui.history.children.length > 5) ui.history.firstElementChild.remove();
}

function updateControls() {
  const playerCanAct = state.turn === "player" && !state.busy && !state.gameOver;
  ui.rollButton.disabled = !playerCanAct;
  ui.holdButton.disabled = !playerCanAct || !state.playerHasRolled;
}

function setTurn(owner) {
  state.turn = owner;
  ui.playerGlow.classList.toggle("is-active", owner === "player");
  ui.opponentGlow.classList.toggle("is-active", owner === "opponent");
  ui.statusChip.classList.toggle("is-thinking", owner === "opponent");
  ui.statusChip.innerHTML = owner === "player" ? "<span></span>你的回合" : "<span></span>机器人思考中";
  updateControls();
}

async function playerRoll() {
  if (state.busy || state.turn !== "player" || state.gameOver) return;
  ensureAudio();
  state.busy = true;
  updateControls();
  setMessage("投掷中", "骰子正在翻滚…");

  const values = [randomDie(), randomDie()];
  await animateDice("player", values);
  const gained = values[0] + values[1];
  state.playerScore += gained;
  state.playerHasRolled = true;
  addHistory("player", values);
  popScore(ui.playerScore, state.playerScore);
  playScoreSound();

  if (state.playerScore > 21) {
    setMessage("爆骰！", `${state.playerScore} 超过了 21`, "opponent");
    ui.tipText.textContent = `${state.playerScore} 点已经超过 21，本局结束`;
    ui.shell.classList.add("screen-shake");
    await wait(620);
    endGame(false, "你超过了 21，机器人获胜。");
    return;
  }

  if (state.playerScore === 21) {
    setMessage("完美点数", "21！机器人只能祈祷你失误了");
    ui.tipText.textContent = "正好 21！保存后机器人无法用有效点数超过你";
    ui.shell.classList.add("perfect-flash");
  } else {
    setMessage(`本次 +${gained}`, state.playerScore >= 18 ? "很接近了，要保存吗？" : "继续投，还是见好就收？");
    ui.tipText.textContent = `当前 ${state.playerScore} 点，距离 21 还差 ${21 - state.playerScore} 点`;
  }

  state.busy = false;
  updateControls();
}

async function holdScore() {
  if (state.busy || state.turn !== "player" || !state.playerHasRolled || state.gameOver) return;
  ensureAudio();
  state.busy = true;
  setTurn("opponent");
  setMessage("点数已保存", `你以 ${state.playerScore} 点向机器人发起挑战`, "opponent");
  ui.tipText.textContent = "机器人不会停手：超过你的点数就赢，超过 21 就输";
  tone(260, .18, "triangle", .04);
  tone(390, .2, "triangle", .035, .1);
  await wait(1000);
  await opponentTurn();
}

async function opponentTurn() {
  while (!state.gameOver) {
    setMessage("机器人回合", `${state.opponentScore} 点，还没超过你…`, "opponent");
    await wait(560);

    const values = [randomDie(), randomDie()];
    setMessage("机器人投掷中", "它没有停手这个选项", "opponent");
    await animateDice("opponent", values);

    const gained = values[0] + values[1];
    state.opponentScore += gained;
    addHistory("opponent", values);
    popScore(ui.opponentScore, state.opponentScore);
    playScoreSound();

    if (state.opponentScore > 21) {
      setMessage("机器人爆骰！", `${state.opponentScore} 超过了 21，你赢了！`);
      ui.shell.classList.add("screen-shake");
      await wait(800);
      endGame(true, "机器人没能及时停手，最终超过了 21。");
      return;
    }

    if (state.opponentScore > state.playerScore) {
      setMessage("机器人反超", `${state.opponentScore} 比 ${state.playerScore} 更大`, "opponent");
      await wait(800);
      endGame(false, "机器人没有超过 21，并且点数已经大于你。");
      return;
    }

    setMessage(`机器人 +${gained}`, state.opponentScore === state.playerScore ? "双方同点，它还会继续投" : "它还在追赶你的点数", "opponent");
    await wait(850);
  }
}

function endGame(playerWon, description) {
  state.gameOver = true;
  state.busy = false;
  ui.playerGlow.classList.remove("is-active");
  ui.opponentGlow.classList.remove("is-active");
  ui.statusChip.classList.remove("is-thinking");
  ui.statusChip.innerHTML = "<span></span>本局结束";
  updateControls();
  playResultSound(playerWon);

  ui.resultCard.classList.toggle("is-loss", !playerWon);
  ui.resultBadge.textContent = playerWon ? "胜" : "负";
  ui.resultKicker.textContent = playerWon ? "LUCKY!" : "SO CLOSE";
  ui.resultTitle.textContent = playerWon ? "你赢了！" : "机器人获胜";
  ui.resultDescription.textContent = description;
  ui.resultPlayerScore.textContent = String(state.playerScore);
  ui.resultOpponentScore.textContent = String(state.opponentScore);
  ui.resultOverlay.hidden = false;
}

function resetGame() {
  state.playerScore = 0;
  state.opponentScore = 0;
  state.playerHasRolled = false;
  state.turn = "player";
  state.busy = false;
  state.gameOver = false;
  ui.playerScore.textContent = "0";
  ui.opponentScore.textContent = "0";
  ui.playerScore.className = "";
  ui.opponentScore.className = "";
  ui.resultOverlay.hidden = true;
  ui.history.innerHTML = '<span class="history-empty">等待第一投…</span>';
  ui.tipText.textContent = "你可以随时保存当前点数，但超过 21 会立即落败";
  ui.shell.classList.remove("screen-shake", "perfect-flash");
  setMessage("你的回合", "投出两颗骰子，向 21 靠近");
  setTurn("player");
}

ui.rollButton.addEventListener("click", playerRoll);
ui.holdButton.addEventListener("click", holdScore);
ui.restartButton.addEventListener("click", resetGame);

ui.soundButton.addEventListener("click", () => {
  state.soundOn = !state.soundOn;
  ui.soundButton.setAttribute("aria-pressed", String(state.soundOn));
  ui.soundButton.setAttribute("aria-label", state.soundOn ? "关闭声音" : "打开声音");
  if (state.soundOn) {
    ensureAudio();
    tone(440, .12, "triangle", .035);
  }
});

ui.helpButton.addEventListener("click", () => ui.rulesDialog.showModal());
ui.closeRulesButton.addEventListener("click", () => ui.rulesDialog.close());
ui.understoodButton.addEventListener("click", () => ui.rulesDialog.close());
ui.rulesDialog.addEventListener("click", (event) => {
  if (event.target === ui.rulesDialog) ui.rulesDialog.close();
});

document.addEventListener("keydown", (event) => {
  if (event.code === "Space" && !ui.rollButton.disabled && !ui.rulesDialog.open) {
    event.preventDefault();
    playerRoll();
  }
  if (event.code === "Enter" && !ui.holdButton.disabled && !ui.rulesDialog.open) holdScore();
});

setTurn("player");
