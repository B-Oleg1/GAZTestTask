create table departments (
 id BIGSERIAL PRIMARY KEY,
 name VARCHAR(255) NOT NULL,
 parent_department_id BIGINT NULL
);

create table employees (
 id BIGSERIAL PRIMARY KEY,
 name VARCHAR(255) NOT NULL,
 position VARCHAR(255) NOT NULL,
 department_id BIGINT NULL
);

ALTER TABLE departments ADD CONSTRAINT "parent_department_id__foreign" FOREIGN KEY (parent_department_id) REFERENCES departments (id) ON DELETE SET NULL;

ALTER TABLE employees ADD CONSTRAINT "department_id__foreign" FOREIGN KEY (department_id) REFERENCES departments (id) ON DELETE SET NULL;

CREATE INDEX departments__parent_department_id__index ON departments (parent_department_id);