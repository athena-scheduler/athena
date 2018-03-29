ALTER TABLE offerings ADD COLUMN IF NOT EXISTS course UUID REFERENCES courses (ID) DEFAULT NULL;

UPDATE offerings SET course = link.course
FROM (SELECT course, offering FROM course_x_offering) AS link
WHERE offerings.id = link.offering;

ALTER TABLE offerings ALTER COLUMN course SET NOT NULL;

DROP TABLE course_x_offering;