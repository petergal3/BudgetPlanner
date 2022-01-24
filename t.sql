with saving_s as (
    select 
        sum (amount) as s
        ,s.date as d
    from dbo.saving s
    group by s.date
),
transactions_i as (
    (select
        sum(amount) as i
        ,t.date as d
    from dbo.transactions t
        where type = 'income'
    group by t.date)),
transactions_e as (
    (select
        sum(amount) as e
        ,t.date as d
    from dbo.transactions t
        where type = 'expense'
    group by t.date))
select 
case 
    when (select s0.s from saving_s s0 where s0.d = (SELECT 
         TOP 1 s1.d 
        FROM saving_s s1 
        where s1.d<i.d and s1.d is not null
        ORDER BY s1.d desc)) is null and s.d is null then 0
     when s.d is null then
        (select s0.s from saving_s s0 where s0.d = (SELECT 
         TOP 1 s1.d 
        FROM saving_s s1 
        where s1.d<i.d and s1.d is not null
        ORDER BY s1.d desc)) else s.s end as saving_amount,
case  
    when (select i0.i from transactions_i i0 where i0.d = (SELECT 
        TOP 1 i.d 
    FROM transactions_i i1 
    where i1.d<s.d and i1.d is not null
       ORDER BY i1.d desc)) is null and  i.d  is null then 0
    when i.d is null then
     (select i0.i from transactions_i i0 where i0.d = (SELECT 
        TOP 1 i.d 
    FROM transactions_i i1 
    where i1.d<s.d and i1.d is not null
       ORDER BY i1.d desc)) else i.i end as income_amount,
case 
    when (select e0.e from transactions_e e0 where e0.d = (SELECT 
        TOP 1 e.d 
    FROM transactions_e e1 
    where e1.d<s.d and e1.d is not null
       ORDER BY e1.d desc)) is null and  e.d  is null then 0
    when e.d is null then
     (select e0.e from transactions_e e0 where e0.d = (SELECT 
        TOP 1 e.d 
    FROM transactions_e e1 
    where e1.d<s.d and e1.d is not null
       ORDER BY e1.d desc)) else e.e end as expense_amount,
     (case  
                when (select i0.i from transactions_i i0 where i0.d = (SELECT 
                    TOP 1 i.d 
                FROM transactions_i i1 
                where i1.d<s.d and i1.d is not null
                   ORDER BY i1.d desc)) is null and  i.d  is null then 0
                when i.d is null then
                 (select i0.i from transactions_i i0 where i0.d = (SELECT 
                    TOP 1 i.d 
                FROM transactions_i i1 
                where i1.d<s.d and i1.d is not null
                   ORDER BY i1.d desc)) else i.i end )
            -
               ( case 
                    when (select e0.e from transactions_e e0 where e0.d = (SELECT 
                        TOP 1 e.d 
                    FROM transactions_e e1 
                    where e1.d<s.d and e1.d is not null
                       ORDER BY e1.d desc)) is null and  e.d  is null then 0
                    when e.d is null then
                     (select e0.e from transactions_e e0 where e0.d = (SELECT 
                        TOP 1 e.d 
                    FROM transactions_e e1 
                    where e1.d<s.d and e1.d is not null
                       ORDER BY e1.d desc)) else e.e end) as balance_value,
case when i.d is null and e.d is null then s.d 
     when i.d is null and s.d is null then e.d
     when e.d is null and s.d is null then i.d
     when i.d is null then e.d
     when e.d is null then i.d
     when s.d is null then i.d
    end as d
from transactions_i i
full outer join transactions_e e on e.d = i.d
full outer join saving_s s on s.d = i.d
order by d





with saving_s as ( select  sum (amount) as s,s.date as d from dbo.saving s group by s.date),transactions_i as ( (select sum(amount) as i ,t.date as d from dbo.transactions t where type = 'income' group by t.date)), transactions_e as (  (select  sum(amount) as e  ,t.date as d  from dbo.transactions t  where type = 'expense' group by t.date)) select  (case    when (select i0.i from transactions_i i0 where i0.d = (SELECT  TOP 1 i.d  FROM transactions_i i1  where i1.d<s.d and i1.d is not null   ORDER BY i1.d desc)) is null and  i.d  is null then 0 when i.d is null then (select i0.i from transactions_i i0 where i0.d = (SELECT  TOP 1 i.d  FROM transactions_i i1  where i1.d<s.d and i1.d is not null  ORDER BY i1.d desc)) else i.i end ) - ( case   when (select e0.e from transactions_e e0 where e0.d = (SELECT   TOP 1 e.d  FROM transactions_e e1 where e1.d<s.d and e1.d is not null  ORDER BY e1.d desc)) is null and  e.d  is null then 0   when e.d is null then  (select e0.e from transactions_e e0 where e0.d = (SELECT TOP 1 e.d  FROM transactions_e e1   where e1.d<s.d and e1.d is not null ORDER BY e1.d desc)) else e.e end) as balance_value from transactions_i i full outer join transactions_e e on e.d = i.d full outer join saving_s s on s.d = i.d order by i.d


with saving_s as ( select  sum (amount) as s,s.date as d from dbo.saving s group by s.date),transactions_i as ( (select sum(amount) as i ,t.date as d from dbo.transactions t where type = 'income' group by t.date)), transactions_e as (  (select  sum(amount) as e  ,t.date as d  from dbo.transactions t  where type = 'expense' group by t.date)) select  case   when (select e0.e from transactions_e e0 where e0.d = (SELECT   TOP 1 e.d  FROM transactions_e e1 where e1.d<s.d and e1.d is not null  ORDER BY e1.d desc)) is null and  e.d  is null then 0   when e.d is null then  (select e0.e from transactions_e e0 where e0.d = (SELECT TOP 1 e.d  FROM transactions_e e1   where e1.d<s.d and e1.d is not null ORDER BY e1.d desc)) else e.e end as expense from transactions_i i full outer join transactions_e e on e.d = i.d full outer join saving_s s on s.d = i.d order by i.d
