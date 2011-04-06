CREATE USER EAI
  IDENTIFIED BY eai
  DEFAULT TABLESPACE USERS
  TEMPORARY TABLESPACE TEMP
  PROFILE DEFAULT
  ACCOUNT UNLOCK;
  -- 2 Roles for EAI 
  GRANT CONNECT TO EAI WITH ADMIN OPTION;
  GRANT DBA TO EAI WITH ADMIN OPTION;
  ALTER USER EAI DEFAULT ROLE ALL;
  -- 1 System Privilege for EAI 
  GRANT UNLIMITED TABLESPACE TO EAI WITH ADMIN OPTION;
  -- 1 Tablespace Quota for EAI 
  ALTER USER EAI QUOTA UNLIMITED ON USERS;
