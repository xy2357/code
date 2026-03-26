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
        for _ in range(num_particles):
            particle = turtle.Turtle()
            particle.shape("circle")import numpy as np
import matplotlib.pyplot as plt
from matplotlib.animation import FuncAnimation

# 设置画布大小和背景颜色
fig, ax = plt.subplots()
fig.set_size_inches(8, 8)
ax.set_xlim(-1, 1)
ax.set_ylim(-1, 1)
ax.set_facecolor("black")

# 初始化烟花粒子数据
num_particles = 100  # 粒子数量
particles, = ax.plot([], [], 'o', color='white', markersize=2)

# 粒子初始位置和速度
angles = np.random.uniform(0, 2 * np.pi, num_particles)
speeds = np.random.uniform(0.2, 0.6, num_particles)
x_data = np.zeros(num_particles)
y_data = np.zeros(num_particles)
vel_x = speeds * np.cos(angles)
vel_y = speeds * np.sin(angles)\n
# 更新动画函数
def update(frame):
    global x_data, y_data, vel_x, vel_y

    # 更新粒子位置
    x_data += vel_x * 0.05
    y_data += vel_y * 0.05

    # 模拟重力效果
    vel_y -= 0.01

    # 更新粒子位置
    particles.set_data(x_data, y_data)
    return particles,

# 动画初始化函数
def init():
    particles.set_data([], [])
    return particles,

# 创建动画
anim = FuncAnimation(fig, update, frames=100, init_func=init, blit=True, interval=50)

plt.show()

            particle.color("white")
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
    firework.explode()import numpy as np
import matplotlib.pyplot as plt
from matplotlib.animation import FuncAnimation

# 设置画布大小和背景颜色
fig, ax = plt.subplots()
fig.set_size_inches(8, 8)
ax.set_xlim(-1, 1)
ax.set_ylim(-1, 1)
ax.set_facecolor("black")

# 初始化烟花粒子数据
num_particles = 100  # 粒子数量
particles, = ax.plot([], [], 'o', color='white', markersize=2)

# 粒子初始位置和速度
angles = np.random.uniform(0, 2 * np.pi, num_particles)
speeds = np.random.uniform(0.2, 0.6, num_particles)
x_data = np.zeros(num_particles)
y_data = np.zeros(num_particles)
vel_x = speeds * np.cos(angles)
vel_y = speeds * np.sin(angles)\n
# 更新动画函数
def update(frame):
    global x_data, y_data, vel_x, vel_y

    # 更新粒子位置
    x_data += vel_x * 0.05
    y_data += vel_y * 0.05

    # 模拟重力效果
    vel_y -= 0.01

    # 更新粒子位置
    particles.set_data(x_data, y_data)
    return particles,

# 动画初始化函数
def init():
    particles.set_data([], [])
    return particles,

# 创建动画
anim = FuncAnimation(fig, update, frames=100, init_func=init, blit=True, interval=50)

plt.show()


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
