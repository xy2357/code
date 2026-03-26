import sys
import pygame
from settings import Settings
from ship import Ship
from alien import Alien
import game_functions as gf
from pygame.sprite import Group
from game_stats import GameStats
from button import Button
from scoreboard import Scoreboard


def run_game():
    # 初始化游戏并创建一个屏幕对象
    pygame.init()
    ai_settings = Settings()
    screen = pygame.display.set_mode((ai_settings.screen_width, ai_settings.screen_height))
    pygame.display.set_caption("Alien Invasion")
    play_button = Button(ai_settings, screen, "Play")

    # 创建飞船
    ship = Ship(ai_settings, screen)
    # alien = Alien(ai_settings, screen)
    # 创建一个子弹编组
    bullets = Group()

    # 创建一个外星人组
    aliens = Group()
    gf.create_fleet(ai_settings, screen, ship, aliens)

    # 创建一个存储游戏信息的实例
    stats = GameStats(ai_settings)
    sb = Scoreboard(ai_settings, screen, stats)

    # 开始游戏的主循环
    while True:
        # 监听键盘和鼠标
        gf.check_events(ai_settings, screen, stats, sb, play_button, ship, aliens, bullets)
        gf.update_screen(ai_settings, screen, stats, sb, ship, aliens, bullets, play_button)
        if stats.game_active:
            ship.update()
            gf.update_bullets(ai_settings, screen, stats, sb,ship, aliens,bullets)
            gf.update_aliens(ai_settings, stats, sb, screen ,ship, aliens, bullets)

run_game()

