with saving_s as (
    select  
        sum (amount) as s,
        s.date as d 
    from dbo.saving s 
    where s.date between '2021-01-01' and '2021-12-30'   
    group by s.date),
transactions_i as (
    (select 
        sum(amount) as i 
        ,t.date as d 
    from dbo.transactions t 
    where type = 'income' 
        and t.date between '2021-01-01' 
        and '2021-12-30' 
        group by t.date)),
transactions_e as (
    (select  
        sum(amount) as e  ,
        t.date as d  
        from dbo.transactions t 
        where type = 'expense' 
            and t.date between '2021-01-01' 
            and '2021-12-30' 
            group by t.date)) 
     select  (case   
        when (select i0.i from transactions_i i0 where i0.d = (SELECT  TOP 1 i.d  FROM transactions_i i1  where i1.d<s.d and i1.d is not null   ORDER BY i1.d desc)) is null and  i.d  is null then 0 
        when i.d is null then (select i0.i from transactions_i i0 where i0.d = (SELECT  TOP 1 i.d  FROM transactions_i i1  where i1.d<s.d and i1.d is not null  ORDER BY i1.d desc)) else i.i end ) - ( case   when (select e0.e from transactions_e e0 where e0.d = (SELECT   TOP 1 e.d  FROM transactions_e e1 where e1.d<s.d and e1.d is not null  ORDER BY e1.d desc)) is null and  e.d  is null then 0   when e.d is null then  (select e0.e from transactions_e e0 where e0.d = (SELECT TOP 1 e.d  FROM transactions_e e1   where e1.d<s.d and e1.d is not null ORDER BY e1.d desc)) else e.e end) as balance_value 
     from transactions_i i 
     full outer join transactions_e e on e.d = i.d 
     full outer join saving_s s on s.d = i.d 
     order by i.d