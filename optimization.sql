CREATE INDEX saving_idx_date ON dbo.saving (date);
CREATE INDEX transactions_idx_type_date ON dbo.transactions (type,date);
CREATE INDEX transactions_idx_date_userid ON dbo.transactions (date,userId);