SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';

SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;


CREATE TABLE address (
    street character varying(255) NOT NULL,
    city character varying(255) NOT NULL,
    postalcode character(6) NOT NULL,
    "number" bigint NOT NULL,
    suffix character varying(255) NOT NULL
);

ALTER TABLE address OWNER TO postgres;

CREATE TABLE user_address (
    user_id bigint NOT NULL,
    postalcode character(6) NOT NULL,
    "number" bigint NOT NULL,
    suffix character varying(255) NOT NULL,
    "type" int NOT NULL
);

ALTER TABLE user_address OWNER TO postgres;

CREATE TABLE product (
    id SERIAL UNIQUE, 
    "name" character varying(255) NOT NULL,
    description character varying(255) NOT NULL,
 buy_price decimal NOT NULL,   price decimal NOT NULL,
    stock bigint NOT NULL,
    category_id bigint NOT NULL,
    image bytea
);

CREATE TABLE category (
    id SERIAL UNIQUE, 
    "name" character varying(255) NOT NULL
);

CREATE TABLE "order" (
    id SERIAL UNIQUE,
    date_time timestamp with time zone DEFAULT '2015-11-27 12:58:49.257+01'::timestamp with time zone NOT NULL,
    user_id bigint NOT NULL,
    order_status bigint
);

ALTER TABLE "order" OWNER TO postgres;


CREATE TABLE orderline (
    product_id bigint NOT NULL,
    order_id bigint not null,
    amount bigint
);

ALTER TABLE orderline OWNER TO postgres;

CREATE TABLE "user" (
    id SERIAL UNIQUE,
    username character varying(255) NOT NULL,
    password_hash bytea NOT NULL,
    password_salt bytea NOT NULL,
    password_iterations integer NOT NULL,
    first_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    email_address character varying(255) NOT NULL,
    date_of_birth date,
    "role" int not null
);

ALTER TABLE "user" OWNER TO postgres;

ALTER TABLE ONLY product ALTER COLUMN id SET DEFAULT nextval('product_id_seq'::regclass);
ALTER TABLE ONLY "user" ALTER COLUMN id SET DEFAULT nextval('user_id_seq'::regclass);
ALTER TABLE ONLY "order" ALTER COLUMN id SET DEFAULT nextval('order_id_seq'::regclass);

SELECT pg_catalog.setval('product_id_seq', 1, false);
SELECT pg_catalog.setval('user_id_seq', 1, false);
SELECT pg_catalog.setval('order_id_seq', 1, false);

ALTER TABLE ONLY address
    ADD CONSTRAINT address_primary_key PRIMARY KEY (postalcode, number, suffix);

ALTER TABLE ONLY user_address
    ADD CONSTRAINT user_address_primary_key PRIMARY KEY (user_id, postalcode, number, suffix);

ALTER TABLE ONLY product
    ADD CONSTRAINT product_primary_key PRIMARY KEY (id);

ALTER TABLE ONLY category
    ADD CONSTRAINT category_primary_key PRIMARY KEY (id);

ALTER TABLE ONLY "order"
    ADD CONSTRAINT order_primary_key PRIMARY KEY (id);

ALTER TABLE ONLY orderline
    ADD CONSTRAINT orderline_primary_key PRIMARY KEY (product_id, order_id);

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_primary_key PRIMARY KEY (id);

CREATE INDEX fki_order_user_foreign_key ON "order" USING btree (user_id);

CREATE INDEX fki_orderline_product_foreign_key ON orderline USING btree (product_id);

CREATE INDEX fki_product_category_foreign_key ON product USING btree (category_id);

CREATE INDEX fki_orderline_order_foreign_key ON orderline USING btree (order_id);

CREATE INDEX fki_user_user_address_foreign_key ON user_address USING btree (postalcode, number, suffix);

CREATE INDEX fki_address_user_address_foreign_key ON user_address USING btree (user_id);

CREATE RULE address_ignore_duplicate_primary_key_inserts AS
    ON INSERT TO address
   WHERE (EXISTS ( SELECT 1
           FROM address
          WHERE (((address.postalcode = new.postalcode) AND (address.number = new.number)) AND ((address.suffix)::text = (new.suffix)::text)))) DO INSTEAD NOTHING;

ALTER TABLE ONLY "order"
    ADD CONSTRAINT order_user_foreign_key FOREIGN KEY (user_id) REFERENCES "user"(id);

ALTER TABLE ONLY product
    ADD CONSTRAINT product_category_foreign_key FOREIGN KEY (category_id) REFERENCES category(id);

ALTER TABLE ONLY orderline
    ADD CONSTRAINT orderline_product_foreign_key FOREIGN KEY (product_id) REFERENCES product(id);

ALTER TABLE ONLY orderline
    ADD CONSTRAINT orderline_order_foreign_key FOREIGN KEY (order_id) REFERENCES "order"(id);

ALTER TABLE ONLY user_address
    ADD CONSTRAINT user_user_address_foreign_key FOREIGN KEY (user_id) REFERENCES "user"(id);

ALTER TABLE ONLY user_address
    ADD CONSTRAINT address_user_address_foreign_key FOREIGN KEY (postalcode, number, suffix) REFERENCES address(postalcode, number, suffix);

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;