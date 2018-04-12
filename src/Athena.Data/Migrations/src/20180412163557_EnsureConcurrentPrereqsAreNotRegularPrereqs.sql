DELETE FROM course_prereqs prereqs
USING course_concurrent_prereqs concurrents
WHERE prereqs.course = concurrents.course AND prereqs.prereq = concurrents.prereq;
