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

DO $$
BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname ILIKE 'DAY_OF_WEEK') then
    CREATE TYPE DAY_OF_WEEK as ENUM(
      'SUNDAY', 'MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SATURDAY'
    );
  END IF;
END$$;

CREATE TABLE IF NOT EXISTS meetings (
  id UUID PRIMARY KEY NOT NULL,
  day DAY_OF_WEEK NOT NULL,
  "time" TIME WITH TIME ZONE NOT NULL,
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
  campus UUID REFERENCES campuses (id) NOT NULL,
  institution UUID REFERENCES institutions (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS student_x_completed_course (
  student UUID REFERENCES students (id) NOT NULL,
  course UUID REFERENCES courses (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS student_x_in_progress_course (
  student UUID REFERENCES students (id) NOT NULL,
  course UUID REFERENCES courses (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS course_x_offering (
  course UUID REFERENCES courses (id) NOT NULL,
  offering UUID REFERENCES offerings (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS course_requirements (
  course UUID REFERENCES courses (id) NOT NULL,
  requirement UUID REFERENCES requirements (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS course_prereqs (
  course UUID REFERENCES courses (id) NOT NULL,
  prereq UUID REFERENCES requirements (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS course_concurrent_prereqs (
  course UUID REFERENCES courses (id) NOT NULL,
  prereq UUID REFERENCES requirements (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS institution_x_student (
  institution UUID REFERENCES institutions (id) NOT NULL,
  student UUID REFERENCES students (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS offering_x_meeting (
  offering UUID REFERENCES offerings (id) NOT NULL,
  meeting UUID REFERENCES meetings (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS instituion_x_program (
  institution UUID REFERENCES institutions (id) NOT NULL,
  program UUID REFERENCES programs (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS program_requirements (
  program UUID REFERENCES programs (id) NOT NULL,
  requirement UUID REFERENCES requirements (id) NOT NULL
);

CREATE TABLE IF NOT EXISTS student_x_program (
  student UUID REFERENCES students (id) NOT NULL,
  program UUID REFERENCES programs (id) NOT NULL
);
