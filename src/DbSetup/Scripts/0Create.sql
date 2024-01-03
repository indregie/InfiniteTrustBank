
CREATE TABLE IF NOT EXISTS public.acc_types
(
    id SERIAL PRIMARY KEY,
    name character varying COLLATE pg_catalog."default" NOT NULL
);

CREATE TABLE IF NOT EXISTS public.transaction_types
(
    id SERIAL PRIMARY KEY,
    name character varying COLLATE pg_catalog."default" NOT NULL
);

CREATE TABLE IF NOT EXISTS public.users
(  
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    name character varying COLLATE pg_catalog."default" NOT NULL,
    address character varying COLLATE pg_catalog."default",
    is_deleted boolean DEFAULT false,
    CONSTRAINT users_pkey PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public.accounts
(
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    balance numeric NOT NULL DEFAULT 0,
    type_id integer NOT NULL,
    user_id uuid NOT NULL,
    is_deleted boolean DEFAULT false,
    CONSTRAINT accounts_pkey PRIMARY KEY (id),
    CONSTRAINT fk_accounts_types FOREIGN KEY (type_id)
        REFERENCES public.acc_types (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS public.transactions
(
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    transaction_type_id integer,
    sum numeric NOT NULL,
    sender_acc_id uuid,
    receiver_acc_id uuid,
    CONSTRAINT transactions_pkey PRIMARY KEY (id),
    CONSTRAINT fk_transactions_transaction_types FOREIGN KEY (transaction_type_id)
        REFERENCES public.transaction_types (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_transactions_sender_accounts FOREIGN KEY (sender_acc_id)
        REFERENCES public.accounts (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_transactions_receiver_accounts FOREIGN KEY (receiver_acc_id)
        REFERENCES public.accounts (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);