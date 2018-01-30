CREATE TABLE IF NOT EXISTS  campuses (
  id UUID PRIMARY KEY NOT NULL,
  name TEXT NOT NULL,
  description TEXT NOT NULL,
  location TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS institutions (
  id UUID PRIMARY KEY NOT NULL,
  name TEXT NOT NULL,
  description TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS courses (
  id UUID PRIMARY KEY NOT NULL,
  name TEXT NOT NULL,
  institution UUID REFERENCES institutions (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS meetings (
  id UUID PRIMARY KEY NOT NULL,
  day INTEGER NOT NULL,
  "time" TIME NOT NULL,
  duration INTERVAL NOT NULL,
  room TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS offerings (
  id UUID PRIMARY KEY NOT NULL,
  campus UUID REFERENCES campuses (id) NOT NULL,
  start DATE NOT NULL,
  "end" DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS programs (
  id UUID PRIMARY KEY NOT NULL,
  name TEXT NOT NULL,
  description TEXT NOT NULL,
  institution UUID REFERENCES institutions (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS requirements (
  id UUID PRIMARY KEY NOT NULL,
  name TEXT NOT NULL,
  description TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS students (
  id UUID PRIMARY KEY NOT NULL,
  name TEXT NOT NULL,
  email TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS campus_x_institution (
  campus UUID NOT NULL REFERENCES campuses (id) ON DELETE CASCADE,
  institution UUID NOT NULL REFERENCES institutions (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS student_x_completed_course (
  student UUID NOT NULL REFERENCES students (id) ON DELETE CASCADE,
  course UUID NOT NULL REFERENCES courses (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS student_x_in_progress_course (
  student UUID NOT NULL REFERENCES students (id) ON DELETE CASCADE,
  course UUID NOT NULL REFERENCES courses (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS course_x_offering (
  course UUID NOT NULL REFERENCES courses (id) ON DELETE CASCADE,
  offering UUID NOT NULL REFERENCES offerings (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS course_requirements (
  course UUID NOT NULL REFERENCES courses (id) ON DELETE CASCADE,
  requirement UUID NOT NULL REFERENCES requirements (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS course_prereqs (
  course UUID NOT NULL REFERENCES courses (id) ON DELETE CASCADE,
  prereq UUID NOT NULL REFERENCES requirements (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS course_concurrent_prereqs (
  course UUID NOT NULL REFERENCES courses (id) ON DELETE CASCADE,
  prereq UUID NOT NULL REFERENCES requirements (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS institution_x_student (
  institution UUID NOT NULL REFERENCES institutions (id) ON DELETE CASCADE,
  student UUID NOT NULL REFERENCES students (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS offering_x_meeting (
  offering UUID NOT NULL REFERENCES offerings (id) ON DELETE CASCADE,
  meeting UUID NOT NULL REFERENCES meetings (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS instituion_x_program (
  institution UUID NOT NULL REFERENCES institutions (id) ON DELETE CASCADE,
  program UUID NOT NULL REFERENCES programs (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS program_requirements (
  program UUID NOT NULL REFERENCES programs (id) ON DELETE CASCADE,
  requirement UUID NOT NULL REFERENCES requirements (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS student_x_program (
  student UUID NOT NULL REFERENCES students (id) ON DELETE CASCADE,
  program UUID NOT NULL REFERENCES programs (id) ON DELETE CASCADE
);
