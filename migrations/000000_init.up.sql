create sequence task_id;
create table if not exists tasks (id int default nextval('task_id') primary key, name varchar);
alter sequence task_id owned by tasks.id;
create sequence user_id;
create table if not exists users (id int default nextval('user_id') primary key,login varchar, password varchar);
alter sequence user_id owned by users.id;