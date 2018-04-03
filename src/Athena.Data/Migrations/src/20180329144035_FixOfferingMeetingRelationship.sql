ALTER TABLE meetings ADD COLUMN IF NOT EXISTS offering UUID REFERENCES offerings (id) ON DELETE CASCADE;

UPDATE meetings SET offering = link.offering
FROM (SELECT offering, meeting FROM offering_x_meeting) AS link
WHERE id = link.meeting;

ALTER TABLE meetings ALTER COLUMN offering SET NOT NULL;

DROP TABLE offering_x_meeting;