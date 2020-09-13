CREATE TABLE messages
(
    id UUID PRIMARY KEY,
    subject TEXT,
    body TEXT,
    date TIMESTAMP WITH TIME ZONE NOT NULL,
    result VARCHAR(16),
    failed_message TEXT
);

CREATE TABLE recipients
(
    message_id UUID,
    email_address VARCHAR(128),
    FOREIGN KEY (message_id) REFERENCES messages (id)
);

