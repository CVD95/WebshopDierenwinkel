--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.5
-- Dumped by pg_dump version 9.4.5
-- Started on 2016-04-04 02:06:34

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 184 (class 3079 OID 11855)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2076 (class 0 OID 0)
-- Dependencies: 184
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 172 (class 1259 OID 33389)
-- Name: address; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE address (
    street character varying(255) NOT NULL,
    city character varying(255) NOT NULL,
    postalcode character(6) NOT NULL,
    number bigint NOT NULL,
    suffix character varying(255) NOT NULL
);


ALTER TABLE address OWNER TO postgres;

--
-- TOC entry 177 (class 1259 OID 33411)
-- Name: category; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE category (
    id integer NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE category OWNER TO postgres;

--
-- TOC entry 176 (class 1259 OID 33409)
-- Name: category_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE category_id_seq OWNER TO postgres;

--
-- TOC entry 2077 (class 0 OID 0)
-- Dependencies: 176
-- Name: category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE category_id_seq OWNED BY category.id;


--
-- TOC entry 179 (class 1259 OID 33419)
-- Name: order; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "order" (
    id integer NOT NULL,
    date_time timestamp with time zone DEFAULT '2015-11-27 11:58:49.257+00'::timestamp with time zone NOT NULL,
    user_id bigint NOT NULL,
    order_status bigint
);


ALTER TABLE "order" OWNER TO postgres;

--
-- TOC entry 178 (class 1259 OID 33417)
-- Name: order_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE order_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE order_id_seq OWNER TO postgres;

--
-- TOC entry 2078 (class 0 OID 0)
-- Dependencies: 178
-- Name: order_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE order_id_seq OWNED BY "order".id;


--
-- TOC entry 180 (class 1259 OID 33426)
-- Name: orderline; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE orderline (
    product_id bigint NOT NULL,
    order_id bigint NOT NULL,
    amount bigint
);


ALTER TABLE orderline OWNER TO postgres;

--
-- TOC entry 183 (class 1259 OID 33494)
-- Name: pay_options; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE pay_options (
    name character varying(255) NOT NULL,
    id integer NOT NULL
);


ALTER TABLE pay_options OWNER TO postgres;

CREATE SEQUENCE pay_options_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE pay_options_id_seq OWNER TO postgres;

--
-- TOC entry 2080 (class 0 OID 0)
-- Dependencies: 181
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE pay_options_id_seq OWNED BY pay_options.id;
--
-- TOC entry 175 (class 1259 OID 33400)
-- Name: product; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE product (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    description character varying(255) NOT NULL,
    buy_price numeric NOT NULL,
    price numeric NOT NULL,
    stock bigint NOT NULL,
    category_id bigint NOT NULL,
    image bytea
);


ALTER TABLE product OWNER TO postgres;

--
-- TOC entry 174 (class 1259 OID 33398)
-- Name: product_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE product_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE product_id_seq OWNER TO postgres;

--
-- TOC entry 2079 (class 0 OID 0)
-- Dependencies: 174
-- Name: product_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE product_id_seq OWNED BY product.id;


--
-- TOC entry 182 (class 1259 OID 33431)
-- Name: user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "user" (
    id integer NOT NULL,
    username character varying(255) NOT NULL,
    password_hash bytea NOT NULL,
    password_salt bytea NOT NULL,
    password_iterations integer NOT NULL,
    first_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    email_address character varying(255) NOT NULL,
    date_of_birth date,
    role integer NOT NULL
);


ALTER TABLE "user" OWNER TO postgres;

--
-- TOC entry 173 (class 1259 OID 33395)
-- Name: user_address; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE user_address (
    user_id bigint NOT NULL,
    postalcode character(6) NOT NULL,
    number bigint NOT NULL,
    suffix character varying(255) NOT NULL,
    type integer NOT NULL
);


ALTER TABLE user_address OWNER TO postgres;

--
-- TOC entry 181 (class 1259 OID 33429)
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE user_id_seq OWNER TO postgres;

--
-- TOC entry 2080 (class 0 OID 0)
-- Dependencies: 181
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE user_id_seq OWNED BY "user".id;


--
-- TOC entry 1919 (class 2604 OID 33414)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY category ALTER COLUMN id SET DEFAULT nextval('category_id_seq'::regclass);


--
-- TOC entry 1921 (class 2604 OID 33442)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "order" ALTER COLUMN id SET DEFAULT nextval('order_id_seq'::regclass);


--
-- TOC entry 1918 (class 2604 OID 33440)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY product ALTER COLUMN id SET DEFAULT nextval('product_id_seq'::regclass);


--
-- TOC entry 1922 (class 2604 OID 33441)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "user" ALTER COLUMN id SET DEFAULT nextval('user_id_seq'::regclass);


--
-- TOC entry 1924 (class 2606 OID 33444)
-- Name: address_primary_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY address
    ADD CONSTRAINT address_primary_key PRIMARY KEY (postalcode, number, suffix);


--
-- TOC entry 1935 (class 2606 OID 33416)
-- Name: category_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY category
    ADD CONSTRAINT category_id_key UNIQUE (id);


--
-- TOC entry 1937 (class 2606 OID 33450)
-- Name: category_primary_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY category
    ADD CONSTRAINT category_primary_key PRIMARY KEY (id);


--
-- TOC entry 1940 (class 2606 OID 33425)
-- Name: order_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "order"
    ADD CONSTRAINT order_id_key UNIQUE (id);


--
-- TOC entry 1942 (class 2606 OID 33452)
-- Name: order_primary_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "order"
    ADD CONSTRAINT order_primary_key PRIMARY KEY (id);


--
-- TOC entry 1946 (class 2606 OID 33454)
-- Name: orderline_primary_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY orderline
    ADD CONSTRAINT orderline_primary_key PRIMARY KEY (product_id, order_id);


--
-- TOC entry 1952 (class 2606 OID 33502)
-- Name: pay_options_primary_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY pay_options
    ADD CONSTRAINT pay_options_primary_key PRIMARY KEY (id);


--
-- TOC entry 1931 (class 2606 OID 33408)
-- Name: product_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY product
    ADD CONSTRAINT product_id_key UNIQUE (id);


--
-- TOC entry 1933 (class 2606 OID 33448)
-- Name: product_primary_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY product
    ADD CONSTRAINT product_primary_key PRIMARY KEY (id);


--
-- TOC entry 1928 (class 2606 OID 33446)
-- Name: user_address_primary_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY user_address
    ADD CONSTRAINT user_address_primary_key PRIMARY KEY (user_id, postalcode, number, suffix);


--
-- TOC entry 1948 (class 2606 OID 33439)
-- Name: user_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_id_key UNIQUE (id);


--
-- TOC entry 1950 (class 2606 OID 33456)
-- Name: user_primary_key; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_primary_key PRIMARY KEY (id);


--
-- TOC entry 1925 (class 1259 OID 33462)
-- Name: fki_address_user_address_foreign_key; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fki_address_user_address_foreign_key ON user_address USING btree (user_id);


--
-- TOC entry 1938 (class 1259 OID 33457)
-- Name: fki_order_user_foreign_key; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fki_order_user_foreign_key ON "order" USING btree (user_id);


--
-- TOC entry 1943 (class 1259 OID 33460)
-- Name: fki_orderline_order_foreign_key; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fki_orderline_order_foreign_key ON orderline USING btree (order_id);


--
-- TOC entry 1944 (class 1259 OID 33458)
-- Name: fki_orderline_product_foreign_key; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fki_orderline_product_foreign_key ON orderline USING btree (product_id);


--
-- TOC entry 1929 (class 1259 OID 33459)
-- Name: fki_product_category_foreign_key; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fki_product_category_foreign_key ON product USING btree (category_id);


--
-- TOC entry 1926 (class 1259 OID 33461)
-- Name: fki_user_user_address_foreign_key; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX fki_user_user_address_foreign_key ON user_address USING btree (postalcode, number, suffix);


--
-- TOC entry 2068 (class 2618 OID 33463)
-- Name: address_ignore_duplicate_primary_key_inserts; Type: RULE; Schema: public; Owner: postgres
--

CREATE RULE address_ignore_duplicate_primary_key_inserts AS
    ON INSERT TO address
   WHERE (EXISTS ( SELECT 1
           FROM address
          WHERE (((address.postalcode = new.postalcode) AND (address.number = new.number)) AND ((address.suffix)::text = (new.suffix)::text)))) DO INSTEAD NOTHING;


--
-- TOC entry 1954 (class 2606 OID 33489)
-- Name: address_user_address_foreign_key; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY user_address
    ADD CONSTRAINT address_user_address_foreign_key FOREIGN KEY (postalcode, number, suffix) REFERENCES address(postalcode, number, suffix) on update cascade;


--
-- TOC entry 1956 (class 2606 OID 33464)
-- Name: order_user_foreign_key; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "order"
    ADD CONSTRAINT order_user_foreign_key FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 1958 (class 2606 OID 33479)
-- Name: orderline_order_foreign_key; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY orderline
    ADD CONSTRAINT orderline_order_foreign_key FOREIGN KEY (order_id) REFERENCES "order"(id);


--
-- TOC entry 1957 (class 2606 OID 33474)
-- Name: orderline_product_foreign_key; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY orderline
    ADD CONSTRAINT orderline_product_foreign_key FOREIGN KEY (product_id) REFERENCES product(id);


--
-- TOC entry 1955 (class 2606 OID 33469)
-- Name: product_category_foreign_key; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY product
    ADD CONSTRAINT product_category_foreign_key FOREIGN KEY (category_id) REFERENCES category(id);


--
-- TOC entry 1953 (class 2606 OID 33484)
-- Name: user_user_address_foreign_key; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY user_address
    ADD CONSTRAINT user_user_address_foreign_key FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2075 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2016-04-04 02:06:35

--
-- PostgreSQL database dump complete
--

