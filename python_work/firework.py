import numpy as np
import turtle
import time

# 初始化屏幕
screen = turtle.Screen()
screen.bgcolor("black")
screen.title("烟花动画")
screen.setup(width=800, height=800)
screen.tracer(0)

# 烟花类
class Firework:
    def __init__(self, num_particles):
        self.particles = []
        self.velocities = []
        self.colors = ["red", "yellow", "blue", "green", "purple", "orange", "white"]
        for _ in range(num_particles):
            particle = turtle.Turtle()
            particle.shape("circle")
            particle.color(np.random.choice(self.colors))
            particle.penup()
            particle.speed(0)
            particle.goto(0, 0)
            particle.hideturtle()
            self.particles.append(particle)

            angle = np.random.uniform(0, 2 * np.pi)
            speed = np.random.uniform(2, 6)
            self.velocities.append([speed * np.cos(angle), speed * np.sin(angle)])

    def explode(self):
        for particle in self.particles:
            particle.showturtle()

    def update(self):
        for i, particle in enumerate(self.particles):
            vel_x, vel_y = self.velocities[i]

            # 更新粒子位置
            x, y = particle.xcor() + vel_x, particle.ycor() + vel_y
            particle.goto(x, y)

            # 模拟重力效果
            self.velocities[i][1] -= 0.1

# 主函数
def main():
    firework = Firework(100)
    firework.explode()

    for _ in range(100):
        firework.update()
        screen.update()
        time.sleep(0.05)

    # 清理屏幕
    for particle in firework.particles:
        particle.hideturtle()

if __name__ == "__main__":
    main()
    screen.mainloop()