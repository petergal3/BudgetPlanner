with saving_s as (select  sum(amount) as s,userId, s.date as d from dbo.saving s where userId = 8 and s.date between '2021-01-01' and '2022-01-01' group by s.date,userId), 
                                                                                transactions_s as ((select sum(amount) as t1, userId, t.date as d from dbo.transactions t where userId = 8 and type = 'income' and t.date between '2021-01-01' and '2022-01-01'  group by t.date,userId)),
                                                                                transactions_e as (  (select  sum(amount) as e  , userId, t.date as d  from dbo.transactions t  where userId = 8 and type = 'expense' and t.date between '2021-01-01' and '2022-01-01' group by t.date,userId)) 
                                                                                 select case when s.d is null then (select s0.s from saving_s s0 where s0.userId = 8 and s0.d = (SELECT TOP 1 s1.d FROM saving_s s1 where s1.userId = 8 and s1.d < t.d and s1.d is not null  ORDER BY s1.d desc)) else s.s end as saving_amount
                                                                                 from transactions_s t full outer join transactions_e e on e.d = t.d full outer join saving_s s on s.d = t.d or s.d = e.d

with saving_s as (select  sum(amount) as s, s.userId, s.date as d from dbo.saving s where userId = 8 and s.date between '2021-01-01' and '2022-01-01' group by s.date, userId), 
                                                                                transactions_s as ((select sum(amount) as t1, userId, t.date as d from dbo.transactions t where userId = 8 and type = 'income' and t.date between '2021-01-01' and '2022-01-01'  group by t.date,userId)),
                                                                                transactions_e as (  (select  sum(amount) as e  , userId, t.date as d  from dbo.transactions t  where userId = 8 and type = 'expense' and t.date between '2021-01-01' and '2022-01-01' group by t.date,userId)) 
                                                                                select case when s.d is null then (select sum(s0.s) from saving_s s0 where userId = 8 and s0.d = (SELECT TOP 1 s1.d FROM saving_s s1 where userId = 8 and s1.d < t.d and s1.d is not null  ORDER BY s1.d desc))
                                                                                else s.s end as saving_amount
                                                                                from transactions_s t full outer join transactions_e e on e.d = t.d full outer join saving_s s on s.d = t.d or s.d = e.d


with saving_s as (select  sum(amount) as s, userId, s.date as d from dbo.saving s where userId = 8 and  s.date between '2021-01-01' and '2022-01-13' group by s.date, userId), transactions_s as ((select sum(amount) as t1, userId, t.date as d from dbo.transactions t where userId = 8 and  type = 'income' and t.date between '2021-01-01' and '2022-01-13'  group by t.date,userId))
select case when s.d is null then (select s0.s from saving_s s0 where userId = 8 and  s0.d = (SELECT TOP 1 s1.d FROM saving_s s1 where userId = 8 and  s1.d < t.d and s1.d is not null  ORDER BY s1.d desc)) else s.s end as saving_amount from transactions_s t full outer join saving_s s on s.d = t.d 

select  sum(amount), t.date from dbo.transactions t where t.userId = 8 group by t.date


select * from dbo.saving where userId = 1