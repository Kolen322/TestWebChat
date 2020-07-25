DROP TABLE IF EXISTS messages;
CREATE TABLE messages(id SERIAL PRIMARY KEY, 
              content VARCHAR(128), dateTime timestamp, number integer);