--
-- PostgreSQL database dump
--

-- Dumped from database version 16.4
-- Dumped by pg_dump version 16.4

-- Started on 2024-11-11 21:22:46

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = on;

DROP DATABASE "MementoMori";
--
-- TOC entry 4809 (class 1262 OID 24996)
-- Name: MementoMori; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "MementoMori" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';


ALTER DATABASE "MementoMori" OWNER TO postgres;

\connect "MementoMori"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = on;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 25021)
-- Name: Cards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Cards" (
    "Id" uuid NOT NULL,
    "Question" text NOT NULL,
    "Description" text NOT NULL,
    "Answer" text NOT NULL,
    "lastInterval" integer,
    "nextShow" date,
    "DeckId" uuid
);


ALTER TABLE public."Cards" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 25009)
-- Name: Decks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Decks" (
    "Id" uuid NOT NULL,
    "CreatorId" uuid,
    "isPublic" boolean NOT NULL,
    "Title" text NOT NULL,
    "Description" text,
    "Rating" double precision NOT NULL,
    "RatingCount" bigint NOT NULL,
    "Modified" date NOT NULL,
    "CardCount" bigint NOT NULL,
    "Tags" integer[]
);


ALTER TABLE public."Decks" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 25002)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "Password" text NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 24997)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4803 (class 0 OID 25021)
-- Dependencies: 218
-- Data for Name: Cards; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."Cards" VALUES ('91cd03ee-8d08-4a15-a228-f4cd43294ae6', 'How many bones are in the human body?', 'Lorem ipsum', '206', NULL, NULL, 'dbba9b7e-6571-4238-ab4c-7bfdae98eee2');
INSERT INTO public."Cards" VALUES ('ad20e7d7-9202-4495-8429-a80219e0d6f3', 'Which organ is the aorta a part of? ', 'Lorem ipsum', 'Heart', NULL, NULL, 'dbba9b7e-6571-4238-ab4c-7bfdae98eee2');


--
-- TOC entry 4802 (class 0 OID 25009)
-- Dependencies: 217
-- Data for Name: Decks; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."Decks" VALUES ('1a36e4d4-b125-4981-b33c-cba922023c43', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Basic Geography Concepts', NULL, 4, 4, '2024-10-02', 30, '{7,14}');
INSERT INTO public."Decks" VALUES ('1fc3c16f-4acb-4e44-b4c6-575c32960227', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Introduction to Math', NULL, 4.4, 15, '2024-10-18', 65, '{5,15}');
INSERT INTO public."Decks" VALUES ('2951e188-eab0-4931-968a-9a03ae43517e', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Languages of the World', NULL, 4.6, 5, '2024-10-10', 55, '{3,15}');
INSERT INTO public."Decks" VALUES ('2dc3fc33-6433-415c-82df-f68d555a9bf4', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Philosophy of Science', NULL, 3.8, 15, '2024-09-30', 35, '{11,17}');
INSERT INTO public."Decks" VALUES ('32abb5d5-cd6d-43a1-b918-ac52e5585622', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'World History Overview', NULL, 4.3, 20, '2024-10-15', 60, '{2,15}');
INSERT INTO public."Decks" VALUES ('41ec0e08-dc55-4ef1-9092-106df083d4cd', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Mathematics 101', NULL, 4.7, 10, '2024-10-16', 70, '{5,14}');
INSERT INTO public."Decks" VALUES ('827785e9-43d5-4df3-8017-b686a1065411', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Introduction to Chemistry', NULL, 4.1, 40, '2024-10-07', 45, '{6,14}');
INSERT INTO public."Decks" VALUES ('8ae6ec34-fcdf-4eab-9fd5-6adf641a459e', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Advanced Physics', NULL, 4.8, 100, '2024-10-17', 75, '{4,16}');
INSERT INTO public."Decks" VALUES ('9a30243e-7599-4422-9039-ad5cba00acce', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Computer Science Basics', NULL, 4.5, 30, '2024-10-08', 55, '{8,14}');
INSERT INTO public."Decks" VALUES ('9c2eb8c3-5950-4121-9a60-c116421a096f', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Literature Analysis', NULL, 4.2, 5, '2024-10-14', 40, '{8,16}');
INSERT INTO public."Decks" VALUES ('a18e7692-262b-4083-ab42-7dbabe1c2b25', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Expert-Level Mathematics', NULL, 4.9, 10, '2024-10-19', 90, '{5,17}');
INSERT INTO public."Decks" VALUES ('a20a3e15-3f0a-4415-8447-f72f2f7aa3f4', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Modern Political Theory', NULL, 4.2, 5, '2024-10-13', 50, NULL);
INSERT INTO public."Decks" VALUES ('aa37d10a-7479-4f6c-92c5-5f1fbeaced0e', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Music Theory', NULL, 5, 1, '2024-09-22', 30, '{10}');
INSERT INTO public."Decks" VALUES ('dbba9b7e-6571-4238-ab4c-7bfdae98eee2', '5fee3187-1954-4b6b-b10a-f5f238590a0f', true, 'Biology for Beginners', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam vitae dui sit amet tortor cursus aliquam. Praesent non lorem eget nunc tempus condimentum. Mauris a justo egestas, aliquet massa et, elementum sem. Nulla vehicula massa magna, in dapibus est hendrerit nec. Aenean eu aliquet dui. Aenean a nunc et augue porta faucibus. Fusce dolor massa, tincidunt id est sed, tincidunt elementum mi. Quisque in massa et orci iaculis placerat eu et nulla. Nam arcu nulla, accumsan non rutrum at, mollis bibendum ligula.

Phasellus ornare euismod quam at tempor. Vivamus ut dapibus lorem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam laoreet fermentum sodales. Mauris bibendum enim eu nisl pellentesque, rhoncus dictum purus ullamcorper. Donec volutpat, magna id mollis viverra, ligula lectus euismod sem, at placerat ex elit et nisi. Cras id interdum nunc. Praesent a condimentum mi, in mollis erat. Suspendisse mattis, massa in ornare.', 4.5, 30, '2024-10-12', 50, '{1,14}');
INSERT INTO public."Decks" VALUES ('5fee3187-1954-4b6b-b10a-f5f238590a0f', NULL, true, 'Art History', NULL, 3.9, 20, '2024-09-27', 35, '{9,2,14}');


--
-- TOC entry 4801 (class 0 OID 25002)
-- Dependencies: 216
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."Users" VALUES ('5fee3187-1954-4b6b-b10a-f5f238590a0f', 'test', 'n4bQgYhMfWWaL+qgxVrQFaO/TxsrC4Is0V1sFbDwCgg=');
INSERT INTO public."Users" VALUES ('829d0643-1d33-4fe0-86e7-09b2cb44bed3', 'test2', 'YDA64iuZiGG847KPM+7BvnWKITyGyTwHbb6fVYwRx1I=');


--
-- TOC entry 4800 (class 0 OID 24997)
-- Dependencies: 215
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" VALUES ('20241111175344_decks,cards,users', '8.0.10');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20241111180313_nullable user', '8.0.10');


--
-- TOC entry 4654 (class 2606 OID 25027)
-- Name: Cards PK_Cards; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cards"
    ADD CONSTRAINT "PK_Cards" PRIMARY KEY ("Id");


--
-- TOC entry 4651 (class 2606 OID 25015)
-- Name: Decks PK_Decks; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Decks"
    ADD CONSTRAINT "PK_Decks" PRIMARY KEY ("Id");


--
-- TOC entry 4648 (class 2606 OID 25008)
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- TOC entry 4646 (class 2606 OID 25001)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4652 (class 1259 OID 25033)
-- Name: IX_Cards_DeckId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Cards_DeckId" ON public."Cards" USING btree ("DeckId");


--
-- TOC entry 4649 (class 1259 OID 25034)
-- Name: IX_Decks_CreatorId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Decks_CreatorId" ON public."Decks" USING btree ("CreatorId");


--
-- TOC entry 4656 (class 2606 OID 25028)
-- Name: Cards FK_Cards_Decks_DeckId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cards"
    ADD CONSTRAINT "FK_Cards_Decks_DeckId" FOREIGN KEY ("DeckId") REFERENCES public."Decks"("Id");


--
-- TOC entry 4655 (class 2606 OID 25035)
-- Name: Decks FK_Decks_Users_CreatorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Decks"
    ADD CONSTRAINT "FK_Decks_Users_CreatorId" FOREIGN KEY ("CreatorId") REFERENCES public."Users"("Id");


-- Completed on 2024-11-11 21:22:47

--
-- PostgreSQL database dump complete
--

