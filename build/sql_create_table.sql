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
);
CREATE TABLE short_link_appication (
    app_id      INT(11)        NOT NULL AUTO_INCREMENT,
    app_code    VARCHAR(255)   NOT NULL,
    app_name    VARCHAR(255)   NOT NULL,
    app_secret  VARCHAR(255)   NOT NULL,
    remark      VARCHAR(500)   NOT NULL,
    create_time TIMESTAMP(0)   NOT NULL,
    update_time TIMESTAMP(0)   NOT NULL,
    status      INT(11)        NOT NULL,
PRIMARY KEY (app_id) USING BTREE
);

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
CREATE TABLE short_link_appication (
    app_id      INT4           NOT NULL,
    app_code    VARCHAR(255)   NOT NULL,
    app_name    VARCHAR(255)   NOT NULL,
    app_secret  VARCHAR(255)   NOT NULL,
    remark      VARCHAR(500)   NOT NULL,
    create_time TIMESTAMP      NOT NULL,
    update_time TIMESTAMP      NOT NULL,
    status      INT4           NOT NULL,
CONSTRAINT pk_short_link_appication PRIMARY KEY (app_id) 
);

-- ----------------------------
-- SQLServer
-- ----------------------------
CREATE TABLE short_link (
    id           INT PRIMARY  KEY IDENTITY(1,1),
    short_url    VARCHAR(255) NOT NULL,
    origin_url   VARCHAR(255) NOT NULL,
    create_time  datetime2(0) NOT NULL,
    update_time  datetime2(0) NOT NULL,
    access_count INT NOT NULL 
);
CREATE TABLE short_link_appication (
    app_id      INT PRIMARY    KEY IDENTITY(1,1),
    app_code    VARCHAR(255)   NOT NULL,
    app_name    VARCHAR(255)   NOT NULL,
    app_secret  VARCHAR(255)   NOT NULL,
    remark      VARCHAR(500)   NOT NULL,
    create_time datetime2(0)   NOT NULL,
    update_time datetime2(0)   NOT NULL,
    status      INT            NOT NULL,
);