--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.5
-- Dumped by pg_dump version 9.4.5
-- Started on 2016-04-04 02:07:06

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

--
-- TOC entry 2069 (class 0 OID 33389)
-- Dependencies: 172
-- Data for Name: address; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY address (street, city, postalcode, number, suffix) FROM stdin;
Wilhelminaplein	Amsterdam	4321IH	25	u
LaanvanMeerdervoort	DenHaag	2564ZB	835	a
Sportlaan	Ooltgensplaat	3257XD	3	a
test	Test	9393IE	4	a
Laand	Japland	1234XD	10	i
\.


--
-- TOC entry 2074 (class 0 OID 33411)
-- Dependencies: 177
-- Data for Name: category; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY category (id, name) FROM stdin;
3	Katten
4	Vissen
5	Apen
8	Vogels
\.


--
-- TOC entry 2085 (class 0 OID 0)
-- Dependencies: 176
-- Name: category_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('category_id_seq', 8, true);


--
-- TOC entry 2079 (class 0 OID 33431)
-- Dependencies: 182
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY "user" (id, username, password_hash, password_salt, password_iterations, first_name, last_name, email_address, date_of_birth, role) FROM stdin;
1	cvd	\\x096480d67048078bd888de73fe2fd8b3ae8ab8a3	\\x8bb0443b5e22e17ddac893debedb83667dde506254f58c08ad4710fbe69eefabee440690bc106dbf24a8d1e38e5ad90859e04c1fc5b231040815ce51829ad098	320615	cvd	cvd	cvd@cvd.nl	1995-06-28	1
3	henk	\\xcb59f35907ef0407e278802ef67ae93e42c0ea45	\\x8932eebcce9448f0b2d4a0f169c59a010d2f6e463a41d7831a6d623899d75bb0ad6291caf6dd51c4954953ab3b59376c6f1a9c64eaa30a8fe7f0ee386989d8ef	321916	henk	henk	henk@henk.nl	1999-02-02	0
4	klaas	\\xecdb878d99860841ac37322df347971299b163d0	\\x602500fe12938f0fb5f572c2364afc6dbee1d445a1b16e64f92e17617f924430981e4a4305b920297c938bb64a6392d08cdc3776b160f5b1ca6610786e3e6550	321468	klaas	klaasjan	klaas@klaasjan.nl	1990-01-04	0
5	Cornelis	\\x9a09119328d7a01a7a76eb39c77f95dd22d3ecc1	\\x028c93004a16cad945882a9488726cf90d15d2d20c9a2443bd03fa57b9833d1b4e7f764dd2d5135b3728129110ba49bd5237f26af9a28928746bf8111561530a	320656	Cornelis	van Dam	cornelis@email.nl	2000-01-10	0
6	TEST	\\x898c4f3214928daa496f2ff551b67379fc30a867	\\xa535951ffbcce337b3bab0408f70015992f621d7c5b95450da5babdd2f54c181b53ff90f16d309911a7a472ecca5729dc982873def85b1a9846142ba3ec22198	320282	TEST	TEST	test@test.nl	2000-10-12	0
2	jap	\\xacfb83f977c426531fe9c785f56fb32ea65511d5	\\x14a340967f6e0ffa59e9bd5d0e6063c7b0a5afe205acc9a4977b9f9f6a78bb3429f90c0392030f4cf1448808999bb127afe698614916ed2d1c983eec3bb89875	321441	jap	jap	jap@jap.nl	1995-10-20	2
\.


--
-- TOC entry 2076 (class 0 OID 33419)
-- Dependencies: 179
-- Data for Name: order; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY "order" (id, date_time, user_id, order_status) FROM stdin;
1	2008-11-11 13:23:44+00	1	1
2	2008-11-11 19:23:44+00	1	1
10	2016-03-19 13:38:50.938895+00	4	1
9	2016-03-18 17:23:02.285436+00	4	1
5	2016-03-18 16:49:38.541076+00	4	1
11	2016-03-30 19:49:25.266412+00	4	0
3	2016-03-18 16:46:59.911414+00	4	3
4	2016-03-18 16:48:26.30108+00	4	3
6	2016-03-18 16:59:15.217815+00	4	3
7	2016-03-18 17:07:39.176831+00	4	3
8	2016-03-18 17:20:39.057317+00	4	3
12	2016-04-03 23:28:37.084217+00	3	6
\.


--
-- TOC entry 2086 (class 0 OID 0)
-- Dependencies: 178
-- Name: order_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('order_id_seq', 12, true);


--
-- TOC entry 2072 (class 0 OID 33400)
-- Dependencies: 175
-- Data for Name: product; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY product (id, name, description, buy_price, price, stock, category_id, image) FROM stdin;
3	Kattenbak 1	kattenbak	8.00	10.00	4	3	\N
4	Kattenbak 2	kattenbak	8.00	10.00	4	3	\N
5	Kattenbrokjes 1	kattenbrokjes	8.00	10.00	4	3	\N
6	Kattenbrokjes 2	kattenbrokjes	8.00	10.00	4	3	\N
7	Kattenspeeltje	muis aan touwtje	1	3	11	3	\N
8	Kattenpaal	voor katten	20	30	6	3	\N
9	Apenbroek	groene broek	10	20	1	5	\N
10	Vissenkom	glazen voorwerp, half rond	20	30	100	4	\N
16	Visvoer	vult lekker de maag van uw vis	3	5	20	4	\N
14	viskom	Brede kom	5.50	10.30	1000	4	\N
\.


--
-- TOC entry 2077 (class 0 OID 33426)
-- Dependencies: 180
-- Data for Name: orderline; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY orderline (product_id, order_id, amount) FROM stdin;
3	1	1
3	2	2
5	1	3
4	2	1
5	9	1
6	10	1
9	11	1
10	11	1
3	12	2
4	12	2
\.


--
-- TOC entry 2080 (class 0 OID 33494)
-- Dependencies: 183
-- Data for Name: pay_options; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY pay_options (name, id) FROM stdin;
ideal	1
paypal	2
creditcard	3
\.


--
-- TOC entry 2087 (class 0 OID 0)
-- Dependencies: 174
-- Name: product_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('product_id_seq', 16, true);


--
-- TOC entry 2070 (class 0 OID 33395)
-- Dependencies: 173
-- Data for Name: user_address; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY user_address (user_id, postalcode, number, suffix, type) FROM stdin;
3	4321IH	25	u	0
4	2564ZB	835	a	0
5	3257XD	3	a	0
2	1234XD	10	i	0
6	9393IE	4	a	0
\.


--
-- TOC entry 2088 (class 0 OID 0)
-- Dependencies: 181
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('user_id_seq', 1, false);


-- Completed on 2016-04-04 02:07:07

--
-- PostgreSQL database dump complete
--

