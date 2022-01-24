with saving_s as (
	select sum (amount) as s, 
	s.date as d from dbo.saving s 
	where s.date between '2021-01-01' and '2021-08-13' group by s.date),
transactions_s as (
	select sum(amount) as t1, 
		t.date as d from dbo.transactions t
		where type = 'income' and t.date  between '2021-01-01' and '2021-08-13' group by t.date) 
select case when s.d is null then (select s0.s from saving_s s0 where s0.d = (SELECT TOP 1 s1.d FROM saving_s s1 where s1.d < t.d and s1.d is not null  ORDER BY s1.d desc)) else s.s end as saving_amount from transactions_s t full outer join saving_s s on s.d = t.d