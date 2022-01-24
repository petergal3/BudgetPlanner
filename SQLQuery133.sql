
--with saving_s as (
--    select 
--        sum (amount) as s
--        ,s.date as d
--    from dbo.saving s
--    group by s.date
--),
--transactions_s as (
--    select
--        sum(amount) as t1
--        ,t.date as d
--    from dbo.transactions t
--        where type = 'income'
--    group by t.date)
--select 
--    case when s.s is null 
--        then (select sum(s1.s) from saving_s s1 where s1.d = (select top 1 sf.d from saving_s sf  where sf.d < s1.d group by sf.d)) end 
--    ,t.t1
--    ,s.d
--    ,t.d
--from saving_s s
--full outer join transactions_s t on t.d = s.d



--with saving_s as (
--    select 
--        sum (amount) as s
--        ,s.date as d
--    from dbo.saving s
--    group by s.date
--),
--transactions_s as (
--    select
--        sum(amount) as t1
--        ,t.date as d
--    from dbo.transactions t
--        where type = 'income'
--    group by t.date)
--select 
--    case when s.d is null then
--   (SELECT TOP 1 s1.d FROM saving_s s1 WHERE s1.d < s.d and s1.d <> null
--   ORDER BY s1.d asc ) else s.d END AS DATENEW
--    ,t.t1
--    ,s.d
--    ,t.d
--from saving_s s
--full outer join transactions_s t on t.d = s.d;

with saving_s as (
    select 
        sum (amount) as s
        ,s.date as d
    from dbo.saving s
    group by s.date
),
transactions_s as (
    (select
        sum(amount) as t1
        ,t.date as d
    from dbo.transactions t
        where type = 'income'
    group by t.date))
select 
case when s.d is null then
     (select s0.s from saving_s s0 where s0.d = (SELECT 
        TOP 1 s1.d 
    FROM saving_s s1 
    where s1.d<t.d and s1.d is not null
       ORDER BY s1.d desc)) else s.s end as saving_amount,
case when (select t0.t1 from transactions_s t0 where t0.d = (SELECT 
        TOP 1 t1.d 
    FROM transactions_s t1 
    where t1.d<s.d and t1.d is not null
       ORDER BY t1.d desc)) is null and t.d is null then 0
when t.d is null then
     (select t0.t1 from transactions_s t0 where t0.d = (SELECT 
        TOP 1 t1.d 
    FROM transactions_s t1 
    where t1.d<s.d and t1.d is not null
       ORDER BY t1.d desc)) else t.t1 end as in_amount,
case when t.d is null then s.d else t.d end as d
from transactions_s t
full outer join saving_s s on s.d = t.d


with saving_s as (select sum (amount) as s, s.date as d from dbo.saving s group by s.date), transactions_s as ((select sum(amount) as t1, t.date as d from dbo.transactions t where type = 'income' group by t.date)) select case when t.d is null then s.d else t.d end as d from transactions_s t full outer join saving_s s on s.d = t.d  
         
 

 with saving_s as (select sum (amount) as s ,s.date as d from dbo.saving s group by s.date ), transactions_s as ( (select
                sum(amount) as t1 ,t.date as d from dbo.transactions t where type = 'income' group by t.date)) select 
                case when (select t0.t1 from transactions_s t0 where t0.d = (SELECT  TOP 1 t1.d   FROM transactions_s t1  where t1.d<s.d and t1.d is not null    ORDER BY t1.d desc)) is null and t.d is null then 0 when t.d is null then  (select t0.t1 from transactions_s t0 where t0.d = (SELECT    TOP 1 t1.d    FROM transactions_s t1   where t1.d<s.d and t1.d is not null  ORDER BY t1.d desc)) else t.t1 end as in_amount  from transactions_s t full outer join saving_s s on s.d = t.d 
