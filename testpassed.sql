with saving_s as (select  sum(amount) as s,userId, s.date as d from dbo.saving s where userId = 1 and s.date between '2021-01-01' and '2021-12-30' group by s.date,userId), 
transactions_s as ((select sum(amount) as t1, userId, t.date as d from dbo.transactions t where userId = 1 and t.date between '2021-01-01' and '2021-12-30'  group by t.date,userId))
select case when s.d is null then (select s0.s from saving_s s0 where userId = 1 and s0.d = (SELECT TOP 1 s1.d FROM saving_s s1 where userId = 1 and s1.d < t.d and s1.d is not null  ORDER BY s1.d desc)) else s.s end as saving_amount
,t.d,s.d, t.t1 from transactions_s t full outer join saving_s s on s.d = t.d order by t.d,s.d

with saving_s as (select sum (amount) as s,s.date as d from dbo.saving s where s.userId = 1 and s.date between '2021-01-01' and '2021-12-30'   group by s.date),
                   transactions_i as ( (select  sum(amount) as i , userId,t.date as d from dbo.transactions t where t.userId = 1 and type = 'income' and t.date between '2021-01-01' and '2021-12-30' group by t.date,userId)), 
                   transactions_e as (  (select  sum(amount) as e  , userId, t.date as d  from dbo.transactions t  where userId = 1 and type = 'expense' and t.date between '2021-01-01' and '2021-12-30' group by t.date,userId)) 
                   select  case  when exists(select 1 from saving_s st where st.d = e.d) then -1
                   when  e.d  is null then 0  
                   when e.d is null then  (select e0.e from transactions_e e0 where userId = 1 and e0.d = (SELECT TOP 1 e.d  FROM transactions_e e1   where userId = 1 and e1.d<s.d and e1.d is not null ORDER BY e1.d desc)) else e.e 
                   end as expense
                  , i.d,i.i, e.d,e.e,s.d ,s.s from transactions_e e full outer join transactions_i i on e.d = i.d  full outer join saving_s s on s.d = i.d or s.d = e.d order by i.d,e.d,s.d

NULL	NULL
NULL	NULL
NULL	NULL
NULL	NULL
NULL	NULL
NULL	NULL
NULL	NULL
2021-08-07	10300000

select * from dbo.transactions where userId = 1 and date between '2021-01-01' and '2021-12-30'

select sum(amount) as t1, userId, t.date as d from dbo.transactions t where userId = 1 and t.date between '2021-01-01' and '2021-12-30'  group by t.date,userId
;
               