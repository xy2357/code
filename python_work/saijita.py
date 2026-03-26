# -*- encoding=utf8 -*-
__author__ = "v_lijianing"

from poco.drivers.unity3d import UnityPoco
from poco.drivers.unity3d.device import UnityEditorWindow
from airtest.core.api import snapshot
import time
import os
import sys
import win32gui
import win32api
import win32con

# specify to work on UnityEditor in this way

dev = UnityEditorWindow()

# make sure your poco-sdk component listens on the following port.
# default value will be 5001. change to any other if your like.
# IP is not used for now
addr = ('127.0.0.1', 5001)

# 指定设备对象初始化unity poco
poco = UnityPoco(addr, device=dev)

# 需要高等级账号才能跑
# 需要高等级账号才能跑
# 需要高等级账号才能跑

has_setteam = True  # 设置一个标志检查是否已经布阵，改过二倍速和自动
has_setgame = True


# 进入关卡
def enter_game():
    poco(text="行动").click([0.5, 0.2])
    time.sleep(4)


# 布阵
def set_team():
    if not hasattr(set_team, "has_setteam") or set_team.has_setteam:
        poco(text="布阵").click([0.5, 0.2])
        poco("txt_automatic").click([0.5, 0.2])
        poco("txt_02").click([0.5, 0.2])
        poco(text="确认").click([0.5, 0.2])
        set_team.has_setteam = False


# 开战
def start_game():
    poco(text="开战").click([0.5, 0.2])
    if poco("btn_02").exists():
        poco("btn_02").click()


# 设置游戏倍速和自动
def set_game():
    if not hasattr(set_game, "has_setgame") or set_game.has_setgame:
        # 先暂停防止杀太快操作不能完成
        poco("tfm_timebar1").click([0.5, 0.5])

        # 二倍数
        if poco("img_Select").exists():
            pass
        else:
            poco("tfm_speed").click([0.3, 0.3])
        # 自动战斗
        if poco(texture="batter_btn_8").exists():
            pass
        else:
            poco("btn_auto").click([0.3, 0.3])

        # 继续
        poco("btn_continue").click([0.5, 0.5])
        set_game.has_setgame = False


# 领取挑战奖励
def gain_challengereward():
    poco("txt_battle").click()
    if poco("btn_receive_lock").exists():
        log("当前无奖励可领取")
        poco(texture="common_btn_return").click()
    else:
        poco(text="全部领取").click()
        snapshot(msg="挑战奖励")
        poco(text="点击空白区域返回").click([0.5, 0.5])
        poco(texture="common_btn_return").click()


# # 失败的处理
# def game_lose():
#     if failed_attempts < max_attempts:
#         log("卡关了重新挑战")
#         snapshot(msg="卡关了重新挑战.")
#         poco("txt_back").click()
#         time.sleep(2)
#         failed_attempts += 1
#     else:
#         log("打不过GG!")
#         snapshot("打不过GG!")


# 结算判断
# def end_judge():
#     tag_again=poco("txt_repeat").wait(120)
#     if poco("txt_victory").exists():
#         poco("btn_back").click()
#         time.sleep(2)
#     else:
#         log("卡关了")
#         snapshot(msg="卡关了重新挑战.")
#         poco("txt_back").click()


# # 领取每个关卡的奖励
# def gain_stagereward(stagenumber):
# try:
#     if poco("Item_stage{}".format(stagenumber)).offspring("btn_ray").exists():
#         poco("SaintGameplayUI").offspring("Item_stage{}".format(stagenumber)).child("tfm_open").click([0.5,0.5])
#         poco("Item_stage{}".format(stagenumber)).offspring("btn_ray").click([0.5,0.5])
#         time.sleep(1)
#         snapshot(msg="奖励截图.")
#         poco(text="获得奖励").click()
#     else:
#         log("领过了跳过")
# except Exception as e:
#     log("奖励GG!")


# # 关卡流程,包含了战斗和领取奖励，为了防止重复使用所以包装成函数了
def game_process(difficulty):
    # 定义几个变量
    current_stage = 1  # 关卡编号
    total_stages = 5  # 总关卡数
    max_attempts = 3  # 最大失败次数
    failed_attempts = 0  # 当前失败次数

    # 难度设置
    if difficulty == "normal":
        poco(text="普通").click()
        log("现在的难度是：" + difficulty)
    elif difficulty == "hard1":
        poco(text="困难").click()
        poco("Dropdown").click()
        poco("Item 0: <color=#e5e1dd>难度等级70%</color>").click()
        log("现在的难度是：" + difficulty)
    elif difficulty == "hard2":
        poco(text="困难").click()
        poco("Dropdown").click()
        poco("Item 1: <color=#e5e1dd>难度等级85%</color>").click()
        log("现在的难度是：" + difficulty)
    elif difficulty == "hard3":
        poco(text="困难").click()
        poco("Dropdown").click()
        poco("Item 2: <color=#e5e1dd>难度等级100%</color>").click()
        log("现在的难度是：" + difficulty)
    elif difficulty == "hard4":
        poco(text="困难").click()
        poco("Dropdown").click()
        poco("Item 3: <color=#e5e1dd>难度等级125%</color>").click()
        log("现在的难度是：" + difficulty)
    elif difficulty == "hard5":
        poco(text="困难").click()
        poco("Dropdown").click()
        poco("Item 4: <color=#8d8d93>难度等级160%</color>").click()
        log("现在的难度是：" + difficulty)
    elif difficulty == "hard6":
        poco(text="困难").click()
        poco("Dropdown").click()
        element_position = poco("Item 3: <color=#e5e1dd>难度等级125%</color>").get_position()
        poco.swipe(element_position, [0.1, 0.5], duration=0.1)
        poco("Item 5: <color=#8d8d93>难度等级200%</color>").click()
        log("现在的难度是：" + difficulty)

    #     try:
    while current_stage <= total_stages:
        poco("SaintGameplayUI").offspring("Item_stage{}".format(current_stage)).child("tfm_open").click([0.5, 0.5])
        if current_stage == 1:  # 只有第一次进才会有布阵。做个区分
            enter_game()
            set_team()
            start_game()
            set_game()
        else:
            enter_game()
            start_game()
        tag_again = poco("txt_repeat").wait(250)
        if poco("txt_victory").exists():  # 胜利结算
            poco("btn_back").click()
            time.sleep(2)
            # 领取奖励
            try:
                if poco("Item_stage{}".format(current_stage)).offspring("btn_ray").exists():
                    poco("SaintGameplayUI").offspring("Item_stage{}".format(current_stage + 1)).offspring(
                        "stage_name").child("txt_name").click([0.5, 0.5])
                    poco("Item_stage{}".format(current_stage)).offspring("btn_ray").click([0.5, 0.5])
                    time.sleep(1)
                    snapshot(msg="奖励截图.")
                    poco(text="获得奖励").click()
                else:  # 以防奖励领取过了
                    log("没找到奖励可能是领过了")
                    snapshot(msg="没找到奖励可能是领过了.")
            except Exception as e:
                log("奖励GG!")
                snapshot(msg="奖励GG!.")

            current_stage += 1
        else:  # 失败重新挑战
            if failed_attempts < max_attempts:
                log("卡关了重新挑战")
                snapshot(msg="卡关了重新挑战.")
                poco("txt_back").click()
                time.sleep(2)
                failed_attempts += 1
            else:
                log("打不过GG!")
                snapshot("打不过GG!")
                sys.exit(0)
    # 普通难度没有挑战奖励所以跳过
    if poco(text="普通难度关卡首通奖励每日重置").exists():
        poco(text="幻域·未开的囚笼").click([0.5, 0.5])
        log("本难度打完了")
    else:
        # 领取挑战奖励
        gain_challengereward()
        poco(text="幻域·未开的囚笼").click([0.5, 0.5])
        log("本难度打完了")


try:
    # 进入副本
    poco("Low").child("module02").click([0.5, 0.5])
    poco(text="演绎回廊Ⅱ").click()
    poco(texture="play_txt10").click([0.1, 0.2])
    # 初始化一下界面，以防出现进入的时候界面不对（点了一个空白位置）
    pos_menu = poco(texture="common_btn_menu_2").get_position()
    pos_need = (pos_menu[0] + 0.05), pos_menu[1]
    poco.click(pos_need)
    # 各难度
    game_process("normal")
    game_process("hard1")
    game_process("hard2")
    game_process("hard3")
    game_process("hard4")
    game_process("hard5")
    game_process("hard6")
# 异常报错
except Exception as e:
    time.sleep(2)
    log("卡关了GG!")
    snapshot(msg="卡关了GG!.")

