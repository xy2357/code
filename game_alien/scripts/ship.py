import pygame
from pygame.sprite import Sprite

class Ship(Sprite):
    def __init__(self, ai_settings, screen):
        super(Ship, self).__init__()
        self.screen =screen
        self.ai_settings = ai_settings
        # 加载飞船图片并将其视为矩形
        self.image = pygame.image.load("../images/ship.bmp")
        self.rect = self.image.get_rect()
        self.screen_rect = self.screen.get_rect()

        # 移动标志
        self.moving_right = False
        self.moving_left = False

        # 将飞船放置屏幕底部中间
        self.rect.centerx = self.screen_rect.centerx
        self.rect.bottom = self.screen_rect.bottom

        self.center = float(self.rect.centerx)

    def update(self):
        if self.moving_right and self.center < self.screen_rect.right - 30:
            self.center += self.ai_settings.ship_speed_factor

        if self.moving_left and self.center > 30:
            self.center -= self.ai_settings.ship_speed_factor

        self.rect.centerx = self.center

    def blitme(self):
        self.screen.blit(self.image, self.rect)

    def center_ship(self):
        self.center = self.screen_rect.centerx