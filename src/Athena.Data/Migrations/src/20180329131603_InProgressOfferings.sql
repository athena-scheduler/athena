DELETE FROM student_x_in_progress_course;
ALTER TABLE student_x_in_progress_course ADD COLUMN offering UUID REFERENCES offerings (id) ON DELETE CASCADE;
ALTER TABLE student_x_in_progress_course ALTER COLUMN offering SET NOT NULL;

ALTER TABLE student_x_in_progress_course DROP CONSTRAINT IF EXISTS student_x_in_progress_course_unique_link;
ALTER TABLE student_x_in_progress_course ADD CONSTRAINT student_x_in_progress_course_unique_link UNIQUE (student, course, offering);
ALTER TABLE student_x_in_progress_course ADD CONSTRAINT student_x_in_progress_course_single_course UNIQUE (student, course);