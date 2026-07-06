-- Id: 2
-- Description: Заполнение таблиц

-- 1. Заполнение таблицы ролей
INSERT INTO roles (id, name) VALUES
    (1, 'Швец'),
    (2, 'Жнец'),
    (3, 'На дуде игрец'),
    (4, 'Кузнец')
ON CONFLICT (id) DO NOTHING;

-- 2. Заполнение таблицы пользователей
INSERT INTO users (id, username) VALUES
    (1, 'брат'),
    (2, 'сват'),
    (3, 'кум'),
    (4, 'сестра'),
    (5, 'отец'),
    (6, 'кент')
ON CONFLICT (id) DO NOTHING;

-- 3. Заполнение таблицы связующей таблицы 
INSERT INTO user_roles (user_id, role_id) VALUES
    (1, 2),
    (2, 3),
    (3, 4),
    (4, 1),
    (4, 3),
    (6, 4),
    (6, 2),
    (5, 1),
    (5, 2),
    (5, 3)
ON CONFLICT (user_id, role_id) DO NOTHING;