CREATE TABLE IF NOT EXISTS users (
  id UUID PRIMARY KEY NOT NULL REFERENCES students(id) ON DELETE CASCADE,
  username TEXT NOT NULL,
  normalized_username TEXT UNIQUE NOT NULL,
  email TEXT NOT NULL,
  normalized_email TEXT NOT NULL,
  email_confirmed BOOLEAN NOT NULL
);

CREATE TABLE IF NOT EXISTS roles (
  id UUID PRIMARY KEY NOT NULL,
  name TEXT NOT NULL,
  normalized_name TEXT UNIQUE NOT NULL
);

INSERT INTO roles (id, name, normalized_name)
  VALUES ('f839e0f0-e29d-4c89-b406-51a52dbb08b5', 'Administrator', 'administrator')
  ON CONFLICT (id) DO UPDATE SET
    name = 'Administrator',
    normalized_name = 'administrator';

CREATE TABLE IF NOT EXISTS external_logins (
  provider_key TEXT NOT NULL,
  login_provider TEXT NOT NULL,
  provider_display_name TEXT NOT NULL,
  user_id UUID NOT NULL  REFERENCES users(id) ON DELETE CASCADE,
  PRIMARY KEY (login_provider, provider_key)
);

CREATE TABLE IF NOT EXISTS user_x_role (
  user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  role_id UUID NOT NULL REFERENCES roles(id) ON DELETE CASCADE
);