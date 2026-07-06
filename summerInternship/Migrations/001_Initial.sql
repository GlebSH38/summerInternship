-- Id: 1
-- Description: Создание таблиц

-- 1. Таблица ролей
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE
);

-- 2. Таблица пользователей
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username TEXT NOT NULL UNIQUE
);

-- 3. Связующая таблица (многие ко многим)
CREATE TABLE user_roles (
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    role_id INTEGER NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
    
    PRIMARY KEY (user_id, role_id)  -- Составной первичный ключ, чтобы не было дубликатов
);