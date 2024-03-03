CREATE TYPE moderation_status AS ENUM ('submitted', 'consideration', 'approved', 'rejected');
CREATE TYPE user_role AS ENUM ('admin', 'user');

CREATE TABLE IF NOT EXISTS users
(
    id       BIGSERIAL PRIMARY KEY,
    name     VARCHAR(50) NOT NULL,
    email    VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(200) NOT NULL,
    role     user_role   NOT NULL DEFAULT 'user'
);

CREATE TABLE IF NOT EXISTS genres
(
    id   SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS books
(
    id           BIGSERIAL PRIMARY KEY,
    title        VARCHAR(100) NOT NULL,
    genre_id     INT REFERENCES genres (id),
    sum_rating   BIGINT                DEFAULT 0 CHECK (sum_rating >= 0),
    count_rating BIGINT                DEFAULT 0 CHECK (count_rating >= 0),
    created_at   TIMESTAMP    NOT NULL DEFAULT NOW(),
    deleted_at   TIMESTAMP
);

CREATE TABLE IF NOT EXISTS authors
(
    id   BIGSERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS authors_books
(
    author_id BIGINT REFERENCES authors (id),
    book_id   BIGINT REFERENCES books (id) ON DELETE CASCADE,
    PRIMARY KEY (author_id, book_id)
);

CREATE TABLE IF NOT EXISTS feedbacks
(
    id               BIGSERIAL PRIMARY KEY,
    book_id          BIGINT REFERENCES books (id),
    feedback         TEXT              NOT NULL,
    given_by_user_id BIGINT REFERENCES users (id),
    estimation       INT CHECK(estimation BETWEEN 1 AND 5),
    status           moderation_status NOT NULL DEFAULT 'submitted',
    created_at       TIMESTAMP         NOT NULL DEFAULT NOW(),
    deleted_at       TIMESTAMP
);

CREATE TABLE IF NOT EXISTS offers
(
    id          UUID PRIMARY KEY     DEFAULT gen_random_uuid(),
    book_id     BIGINT REFERENCES books (id),
    owner_id    BIGINT REFERENCES users (id),
    description TEXT,
    price       DECIMAL(10, 2) check (price >= 0),
    city        VARCHAR(30) NOT NULL,
    picture     TEXT,
    created_at  TIMESTAMP   NOT NULL DEFAULT NOW(),
    deleted_at  TIMESTAMP
);

CREATE TABLE IF NOT EXISTS offers_collector
(
    id          UUID PRIMARY KEY           DEFAULT gen_random_uuid(),
    owner_id    BIGINT REFERENCES users (id),
    title       VARCHAR(100)      NOT NULL,
    authors     TEXT              NOT NULL,
    description TEXT,
    price       DECIMAL(10, 2) check (price >= 0),
    city        VARCHAR(30)       NOT NULL,
    status      moderation_status NOT NULL DEFAULT 'submitted',
    reject_reason VARCHAR(200),
    picture     TEXT,
    created_at  TIMESTAMP         NOT NULL DEFAULT NOW(),
    deleted_at  TIMESTAMP
);

CREATE TABLE IF NOT EXISTS offers_users -- favorites
(
    user_id  BIGINT REFERENCES users (id),
    offer_id UUID REFERENCES offers (id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, offer_id)
);

CREATE TABLE chats (
   chat_id BIGSERIAL PRIMARY KEY,
   user1_id BIGINT NOT NULL,
   user2_id BIGINT NOT NULL,
   created_at TIMESTAMP NOT NULL DEFAULT NOW(),
   FOREIGN KEY (user1_id) REFERENCES users(id) ON DELETE CASCADE,
   FOREIGN KEY (user2_id) REFERENCES users(id) ON DELETE CASCADE,
   CHECK (user1_id < user2_id),
   UNIQUE (user1_id, user2_id) -- гарантирует, что пары пользователей уникальны
);

CREATE UNIQUE INDEX idx_user_pairs ON chats(user1_id, user2_id);

CREATE TABLE messages (
  message_id SERIAL PRIMARY KEY,
  chat_id BIGINT REFERENCES chats(chat_id) ON DELETE CASCADE,
  user_id BIGINT REFERENCES users(id) ON DELETE CASCADE,
  text TEXT NOT NULL,
  sent_at TIMESTAMP NOT NULL DEFAULT NOW()
);
