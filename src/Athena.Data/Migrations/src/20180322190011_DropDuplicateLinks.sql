DELETE FROM campus_x_institution T1 USING campus_x_institution T2
WHERE T1.CTID < T2.CTID
  AND T1.campus = T2.campus
  AND T1.institution = T2.institution;

ALTER TABLE campus_x_institution DROP CONSTRAINT IF EXISTS campus_x_institution_unique_link;
ALTER TABLE campus_x_institution ADD CONSTRAINT campus_x_institution_unique_link UNIQUE (campus, institution);


DELETE FROM student_x_completed_course T1 USING student_x_completed_course T2
WHERE T1.CTID < T2.CTID
  AND T1.student = T2.student
  AND T1.course = T2.course;

ALTER TABLE student_x_completed_course DROP CONSTRAINT IF EXISTS student_x_completed_course_unique_link;
ALTER TABLE student_x_completed_course ADD CONSTRAINT student_x_completed_course_unique_link UNIQUE (student, course);


DELETE FROM student_x_in_progress_course T1 USING student_x_in_progress_course T2
WHERE T1.CTID < T2.CTID
  AND T1.student = T2.student
  AND T1.course = T2.course;

ALTER TABLE student_x_in_progress_course DROP CONSTRAINT IF EXISTS student_x_in_progress_course_unique_link;
ALTER TABLE student_x_in_progress_course ADD CONSTRAINT student_x_in_progress_course_unique_link UNIQUE (student, course);


DELETE FROM course_x_offering T1 USING course_x_offering T2
WHERE T1.CTID < T2.CTID
  AND T1.course = T2.course
  AND T1.offering = T2.offering;

ALTER TABLE course_x_offering DROP CONSTRAINT IF EXISTS course_x_offering_unique_link;
ALTER TABLE course_x_offering ADD CONSTRAINT course_x_offering_unique_link UNIQUE (course, offering);


DELETE FROM course_requirements T1 USING course_requirements T2
WHERE T1.CTID < T2.CTID
  AND T1.course = T2.course
  AND T1.requirement = T2.requirement;

ALTER TABLE course_requirements DROP CONSTRAINT IF EXISTS course_requirements_unique_link;
ALTER TABLE course_requirements ADD CONSTRAINT course_requirements_unique_link UNIQUE (course, requirement);


DELETE FROM course_prereqs T1 USING course_prereqs T2
WHERE T1.CTID < T2.CTID
  AND T1.course = T2.course
  AND T1.prereq = T2.prereq;

ALTER TABLE course_prereqs DROP CONSTRAINT IF EXISTS course_prereqs_unique_link;
ALTER TABLE course_prereqs ADD CONSTRAINT course_prereqs_unique_link UNIQUE (course, prereq);


DELETE FROM course_concurrent_prereqs T1 USING course_concurrent_prereqs T2
WHERE T1.CTID < T2.CTID
  AND T1.course = T2.course
  AND T1.prereq = T2.prereq;

ALTER TABLE course_concurrent_prereqs DROP CONSTRAINT IF EXISTS course_concurrent_prereqs_unique_link;
ALTER TABLE course_concurrent_prereqs ADD CONSTRAINT course_concurrent_prereqs_unique_link UNIQUE (course, prereq);


DELETE FROM institution_x_student T1 USING institution_x_student T2
WHERE T1.CTID < T2.CTID
  AND T1.institution = T2.institution
  AND T1.student = T2.student;

ALTER TABLE institution_x_student DROP CONSTRAINT IF EXISTS institution_x_student_unique_link;
ALTER TABLE institution_x_student ADD CONSTRAINT institution_x_student_unique_link UNIQUE (institution, student);


DELETE FROM offering_x_meeting T1 USING offering_x_meeting T2
WHERE T1.CTID < T2.CTID
  AND T1.offering = T2.offering
  AND T1.meeting = T2.meeting;

ALTER TABLE offering_x_meeting DROP CONSTRAINT IF EXISTS offering_x_meeting_unique_link;
ALTER TABLE offering_x_meeting ADD CONSTRAINT offering_x_meeting_unique_link UNIQUE (offering, meeting);


DELETE FROM program_requirements T1 USING program_requirements T2
WHERE T1.CTID < T2.CTID
  AND T1.program = T2.program
  AND T1.requirement = T2.requirement;

ALTER TABLE program_requirements DROP CONSTRAINT IF EXISTS program_requirements_unique_link;
ALTER TABLE program_requirements ADD CONSTRAINT program_requirements_unique_link UNIQUE (program, requirement);


DELETE FROM student_x_program T1 USING student_x_program T2
WHERE T1.CTID < T2.CTID
  AND T1.student = T2.student
  AND T1.program = T2.program;

ALTER TABLE student_x_program DROP CONSTRAINT IF EXISTS student_x_program_unique_link;
ALTER TABLE student_x_program ADD CONSTRAINT student_x_program_unique_link UNIQUE (student, program);


DELETE FROM user_x_role T1 USING user_x_role T2
WHERE T1.CTID < T2.CTID
  AND T1.user_id = T2.user_id
  AND T1.role_id = T2.role_id;

ALTER TABLE user_x_role DROP CONSTRAINT IF EXISTS user_x_role_unique_link;
ALTER TABLE user_x_role ADD CONSTRAINT user_x_role_unique_link UNIQUE (user_id, role_id);


DROP TABLE IF EXISTS instituion_x_program;
DROP TABLE IF EXISTS institution_x_program;
