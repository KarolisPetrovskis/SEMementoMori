--
-- PostgreSQL database dump
--

-- Dumped from database version 16.4
-- Dumped by pg_dump version 16.4

-- Started on 2024-10-22 17:10:15

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
-- TOC entry 4800 (class 1262 OID 16448)
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

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 4801 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 217 (class 1259 OID 16461)
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
-- TOC entry 216 (class 1259 OID 16454)
-- Name: Decks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Decks" (
    "Id" uuid NOT NULL,
    "creatorId" uuid NOT NULL,
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
-- TOC entry 215 (class 1259 OID 16449)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4794 (class 0 OID 16461)
-- Dependencies: 217
-- Data for Name: Cards; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."Cards" VALUES ('91cd03ee-8d08-4a15-a228-f4cd43294ae6', 'How many bones are in the human body?', 'Lorem ipsum', '206', NULL, NULL, 'dbba9b7e-6571-4238-ab4c-7bfdae98eee2');
INSERT INTO public."Cards" VALUES ('ad20e7d7-9202-4495-8429-a80219e0d6f3', 'Which organ is the aorta a part of? ', 'Lorem ipsum', 'Heart', NULL, NULL, 'dbba9b7e-6571-4238-ab4c-7bfdae98eee2');


--
-- TOC entry 4793 (class 0 OID 16454)
-- Dependencies: 216
-- Data for Name: Decks; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."Decks" VALUES ('02baefed-c9c5-49b9-b182-846380b70e13', 'a1afdf90-6d19-4654-8385-41bab0cc432e', true, 'Art History', NULL, 3.9, 20, '2024-09-27', 35, '{9,2,14}');
INSERT INTO public."Decks" VALUES ('1a36e4d4-b125-4981-b33c-cba922023c43', '9915e44c-d3a7-425f-bf66-f34cffadbbe7', true, 'Basic Geography Concepts', NULL, 4, 4, '2024-10-02', 30, '{7,14}');
INSERT INTO public."Decks" VALUES ('1fc3c16f-4acb-4e44-b4c6-575c32960227', '455c1896-5d16-4186-9b70-81f27e9b239c', true, 'Introduction to Math', NULL, 4.4, 15, '2024-10-18', 65, '{5,15}');
INSERT INTO public."Decks" VALUES ('2951e188-eab0-4931-968a-9a03ae43517e', '065eaeb0-0b15-4c9b-ae55-ef2f702f0700', true, 'Languages of the World', NULL, 4.6, 5, '2024-10-10', 55, '{3,15}');
INSERT INTO public."Decks" VALUES ('2dc3fc33-6433-415c-82df-f68d555a9bf4', 'd060861d-03c7-45bc-81fc-01a0c9b3f748', true, 'Philosophy of Science', NULL, 3.8, 15, '2024-09-30', 35, '{11,17}');
INSERT INTO public."Decks" VALUES ('32abb5d5-cd6d-43a1-b918-ac52e5585622', '15ef5a48-afe5-4bc8-9e69-969360cef6a5', true, 'World History Overview', NULL, 4.3, 20, '2024-10-15', 60, '{2,15}');
INSERT INTO public."Decks" VALUES ('41ec0e08-dc55-4ef1-9092-106df083d4cd', '180bdc61-cf73-4293-aea2-241535ee9926', true, 'Mathematics 101', NULL, 4.7, 10, '2024-10-16', 70, '{5,14}');
INSERT INTO public."Decks" VALUES ('827785e9-43d5-4df3-8017-b686a1065411', '21c94e2b-25be-4f6e-880a-b84ca10f6531', true, 'Introduction to Chemistry', NULL, 4.1, 40, '2024-10-07', 45, '{6,14}');
INSERT INTO public."Decks" VALUES ('8ae6ec34-fcdf-4eab-9fd5-6adf641a459e', 'd0688e47-52a9-4b00-99fb-2a7a82183955', true, 'Advanced Physics', NULL, 4.8, 100, '2024-10-17', 75, '{4,16}');
INSERT INTO public."Decks" VALUES ('9a30243e-7599-4422-9039-ad5cba00acce', 'b90935c2-0bf1-4269-8241-b070cbbc7c8e', true, 'Computer Science Basics', NULL, 4.5, 30, '2024-10-08', 55, '{8,14}');
INSERT INTO public."Decks" VALUES ('9c2eb8c3-5950-4121-9a60-c116421a096f', '40092c05-3dc6-41f7-8e0f-8f85e577beb0', true, 'Literature Analysis', NULL, 4.2, 5, '2024-10-14', 40, '{8,16}');
INSERT INTO public."Decks" VALUES ('a18e7692-262b-4083-ab42-7dbabe1c2b25', 'fb73e73e-ecb1-4bee-adbd-cd6a8e534715', true, 'Expert-Level Mathematics', NULL, 4.9, 10, '2024-10-19', 90, '{5,17}');
INSERT INTO public."Decks" VALUES ('a20a3e15-3f0a-4415-8447-f72f2f7aa3f4', '98856a3d-d365-411e-99ac-29f19889c137', true, 'Modern Political Theory', NULL, 4.2, 5, '2024-10-13', 50, NULL);
INSERT INTO public."Decks" VALUES ('aa37d10a-7479-4f6c-92c5-5f1fbeaced0e', '6e4072d3-3024-4619-b61d-defe49bfcd56', true, 'Music Theory', NULL, 5, 1, '2024-09-22', 30, '{10}');
INSERT INTO public."Decks" VALUES ('dbba9b7e-6571-4238-ab4c-7bfdae98eee2', '7c69faeb-2d3c-487d-84cd-e424c7a54eac', true, 'Biology for Beginners', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam vitae dui sit amet tortor cursus aliquam. Praesent non lorem eget nunc tempus condimentum. Mauris a justo egestas, aliquet massa et, elementum sem. Nulla vehicula massa magna, in dapibus est hendrerit nec. Aenean eu aliquet dui. Aenean a nunc et augue porta faucibus. Fusce dolor massa, tincidunt id est sed, tincidunt elementum mi. Quisque in massa et orci iaculis placerat eu et nulla. Nam arcu nulla, accumsan non rutrum at, mollis bibendum ligula.

Phasellus ornare euismod quam at tempor. Vivamus ut dapibus lorem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam laoreet fermentum sodales. Mauris bibendum enim eu nisl pellentesque, rhoncus dictum purus ullamcorper. Donec volutpat, magna id mollis viverra, ligula lectus euismod sem, at placerat ex elit et nisi. Cras id interdum nunc. Praesent a condimentum mi, in mollis erat. Suspendisse mattis, massa in ornare.', 4.5, 30, '2024-10-12', 50, '{1,14}');


--
-- TOC entry 4792 (class 0 OID 16449)
-- Dependencies: 215
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" VALUES ('20241022114033_InitialMigration', '8.0.10');


--
-- TOC entry 4647 (class 2606 OID 16467)
-- Name: Cards PK_Cards; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cards"
    ADD CONSTRAINT "PK_Cards" PRIMARY KEY ("Id");


--
-- TOC entry 4644 (class 2606 OID 16460)
-- Name: Decks PK_Decks; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Decks"
    ADD CONSTRAINT "PK_Decks" PRIMARY KEY ("Id");


--
-- TOC entry 4642 (class 2606 OID 16453)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4645 (class 1259 OID 16473)
-- Name: IX_Cards_DeckId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Cards_DeckId" ON public."Cards" USING btree ("DeckId");


--
-- TOC entry 4648 (class 2606 OID 16468)
-- Name: Cards FK_Cards_Decks_DeckId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cards"
    ADD CONSTRAINT "FK_Cards_Decks_DeckId" FOREIGN KEY ("DeckId") REFERENCES public."Decks"("Id");


-- Completed on 2024-10-22 17:10:15

--
-- PostgreSQL database dump complete
--

