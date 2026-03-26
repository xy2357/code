from operator import truediv

import pygame
from pygame.sprite import Sprite

class Alien(Sprite):
    def __init__(self, ai_setting, screen):
        super().__init__()
        self.screen = screen
        self.ai_setting = ai_setting

        # 加载外星人图片并设置其rect属性
        self.image = pygame.image.load("../images/alien.bmp")
        self.rect = self.image.get_rect()

        # 外星人出生位置都是左上角
        self.rect.x = self.rect.width
        self.rect.y = self.rect.height

        #存储外新人的位置
        self.x = float(self.rect.x)

    def blitme(self):
        # 在指定位置放置机器人
        self.screen.blit(self.image, self.rect)

    def update(self):
        # 在指定位置放置机器人
        self.x += self.ai_setting.alien_speed_factor * self.ai_setting.fleet_direction
        self.rect.x = self.x

    def check_edges(self):
        screen_rect = self.screen.get_rect()
        if self.rect.right >= screen_rect.right:
            return True
        elif self.rect.left <= 0:
            return True
