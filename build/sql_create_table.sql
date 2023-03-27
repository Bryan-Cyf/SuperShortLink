-- ----------------------------
-- MySQL
-- ----------------------------
CREATE TABLE short_link (
    id            INT(11)      NOT NULL AUTO_INCREMENT,
    short_url     VARCHAR(255) NOT NULL,
    origin_url    VARCHAR(255) NOT NULL,
    create_time   TIMESTAMP(0) NOT NULL,
    update_time   TIMESTAMP(0) NOT NULL,
    access_count  INT(11)      NOT NULL,
PRIMARY KEY (id) USING BTREE 
)

-- ----------------------------
-- PostgreSQL
-- ----------------------------
CREATE TABLE short_link (
    id            SERIAL        NOT NULL,
    short_url     VARCHAR(128)  NOT NULL,
    origin_url    VARCHAR(128)  NOT NULL,
    create_time   TIMESTAMP     NOT NULL,
    update_time   TIMESTAMP     NOT NULL,
    access_count  INT4          NOT NULL
CONSTRAINT pk_short_link PRIMARY KEY (id) 
);

-- ----------------------------
-- SQLServer
-- ----------------------------
CREATE TABLE short_link (
    id           INT PRIMARY KEY IDENTITY(1,1),
    short_url    VARCHAR(255) NOT NULL,
    origin_url   VARCHAR(255) NOT NULL,
    create_time  datetime2(0) NOT NULL,
    update_time  datetime2(0) NOT NULL,
    access_count INT NOT NULL 
);