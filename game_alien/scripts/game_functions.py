import sys
import pygame
from bullet import Bullet
from alien import Alien
from time import sleep

def check_keydown_events(event, ai_setting, screen, ship, bullets):
    if event.key in (pygame.K_RIGHT, pygame.K_d):
        ship.moving_right = True
    elif event.key in (pygame.K_LEFT, pygame.K_a):
        ship.moving_left = True
    elif event.key == pygame.K_SPACE:
        # 创建一个子弹
        fir_bullet(ai_setting, screen, ship, bullets)
    elif event.key == pygame.K_q:
        # 退出游戏
        pygame.quit()
        sys.exit()

def fir_bullet(ai_setting, screen, ship, bullets):
    if len(bullets) < ai_setting.bullets_allowed:
        new_bullet = Bullet(ai_setting, screen, ship)
        bullets.add(new_bullet)

def check_keyup_events(event, ship):
    if event.key in (pygame.K_RIGHT, pygame.K_d):
        ship.moving_right = False
    elif event.key in (pygame.K_LEFT, pygame.K_a):
        ship.moving_left = False

def check_events(ai_settings, screen, stats, sb, play_button, ship, aliens, bullets):
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            sys.exit()
        elif event.type == pygame.KEYDOWN:
            check_keydown_events(event, ai_settings, screen, ship, bullets)
        elif event.type == pygame.KEYUP:
            check_keyup_events(event, ship)
        elif event.type == pygame.MOUSEBUTTONDOWN:
            mouse_x, mouse_y = pygame.mouse.get_pos()
            check_play_button(ai_settings, screen, stats, sb, play_button, ship, aliens, bullets, mouse_x, mouse_y)

def check_play_button(ai_settings, screen, stats, sb, play_button, ship, aliens, bullets, mouse_x, mouse_y):
    button_clicked = play_button.rect.collidepoint(mouse_x, mouse_y)
    if button_clicked and not stats.game_active:
        ai_settings.initialize_dynamic_settings()
        pygame.mouse.set_visible(False)
        # 重置游戏统计信息
        stats.reset_stats()
        stats.game_active = True

        # 重置积分图像
        sb.prep_score()
        sb.prep_high_score()
        sb.prep_level()
        sb.prep_ships()

        aliens.empty()
        bullets.empty()

        create_fleet(ai_settings, screen, ship, aliens)
        ship.center_ship()

def update_screen(ai_settings, screen, stats, sb, ship, aliens, bullets, play_button):
    # 每次循环都重新绘制屏幕
    screen.fill(ai_settings.bg_color)
    ship.blitme()
    aliens.draw(screen)
    for bullet in bullets.sprites():
        bullet.draw_bullet()

    # 显示得分
    sb.show_score()

    # 绘制开始按钮
    if not stats.game_active:
        play_button.draw_button()

    # 让绘制的屏幕可见
    pygame.display.flip()

def update_bullets(ai_settings, screen, stats, sb,ship, aliens,bullets):
    # 更新子弹位置，并删除子弹
    bullets.update()
    for bullet in bullets.copy():
        if bullet.rect.bottom < 0:
            bullets.remove(bullet)
        print(len(bullets))
        check_bullet_alien_collisions(ai_settings, screen, stats, sb,ship, aliens,bullets)

def check_bullet_alien_collisions(ai_settings, screen, stats, sb,ship, aliens,bullets):
    collisions = pygame.sprite.groupcollide(bullets, aliens, True, True)

    if collisions:
        for aliens in collisions.values():
            stats.score += ai_settings.alien_points * len(aliens)
            sb.prep_score()
        check_high_score(stats, sb)

    if len(aliens) == 0:
        bullets.empty()
        ai_settings.increase_speed()
        # 提高等级
        stats.level += 1
        sb.prep_level()
        create_fleet(ai_settings, screen, ship, aliens)

def get_number_aliens_x(ai_settings, alien_width):
    # 计算一行能放下多少个外星人
    available_space_x = ai_settings.screen_width - 2 * alien_width
    number_alien_x = int(available_space_x / (2 * alien_width))
    return number_alien_x

def get_number_rows(ai_settings, ship_height, alien_height):
    # 计算一行能放下多少个外星人
    available_space_y = ai_settings.screen_height - 3 * alien_height - ship_height
    number_rows = int(available_space_y / (2 * alien_height))
    return number_rows

def create_alien(ai_settings, screen, aliens, alien_number, row_number):
    alien = Alien(ai_settings, screen)
    alien_width = alien.rect.width
    alien_height = alien.rect.height
    alien.x = alien_width + 2 * alien_width * alien_number
    alien.y = alien_height + 2 * alien_height * row_number
    alien.rect.x = alien.x
    alien.rect.y = alien.y
    aliens.add(alien)

def create_fleet(ai_settings, screen, ship, aliens):
    alien = Alien(ai_settings, screen)
    number_alien_x = get_number_aliens_x(ai_settings, alien.rect.width)
    number_rows = get_number_rows(ai_settings, ship.rect.height, alien.rect.height)
    # 创建一行外星人
    for row_number in range(number_rows):
        for alien_number in range(number_alien_x):
            create_alien(ai_settings, screen, aliens, alien_number, row_number)

def ship_hit(ai_settings, stats, sb, screen ,ship, aliens, bullets):
    if stats.ship_left > 0:
        stats.ship_left -= 1

        sb.prep_ships()

        # 清空外星人和子弹
        aliens.empty()
        bullets.empty()

        # 创建一圈新的外星人，并把飞船放置中间位置
        create_fleet(ai_settings, screen, ship, aliens)
        ship.center_ship()

        # 暂停0。5秒
        sleep(0.5)
    else:
        stats.game_active = False
        pygame.mouse.set_visible(True)

def check_aliens_bottom(ai_settings, stats, sb, screen ,ship, aliens, bullets):
    screen_rect = screen.get_rect()
    for alien in aliens.sprites():
        if alien.rect.bottom >= screen_rect.bottom:
            ship_hit(ai_settings, stats, sb, screen ,ship, aliens, bullets)
            break

def update_aliens(ai_settings, stats, sb, screen ,ship, aliens, bullets):
    # 创建飞船
    check_fleet_edges(ai_settings, aliens)
    check_aliens_bottom(ai_settings, stats, sb, screen, ship, aliens, bullets)
    aliens.update()

    # 检测外星人和飞船之间的碰撞、
    if pygame.sprite.spritecollideany(ship, aliens):
        ship_hit(ai_settings, stats, sb, screen ,ship, aliens, bullets)

def check_fleet_edges(ai_settings, aliens):
    # 当外外星人到达边缘时的措施
    for alien in aliens.sprites():
        if alien.check_edges():
            change_fleet_direction(ai_settings, aliens)
            break

def change_fleet_direction(ai_settings, aliens):
    # 将外星人下移并改变他们的方向
    for alien in aliens.sprites():
        alien.rect.y += ai_settings.fleet_drop_speed
    ai_settings.fleet_direction *= -1

def check_high_score(stats, sb):
    if stats.score > stats.high_score:
        stats.high_score = stats.score
        sb.prep_high_score()
