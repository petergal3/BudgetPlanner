Select 
    case when 
    t.category = 'saving' or t.category = 'saving withdrawal' then t.amount
    else (select sum(t1.amount) from dbo.transactions t1 where t1.type = 'income' and
            (t1.category <> 'saving' or t1.category <> 'saving withdrawal') 
            and t1.date >= t.date and t.date <= (select top 1 t_filter.date from dbo.transactions t_filter
                                            where t_filter.type = 'income' and (t_filter.category = 'saving'  or  t_filter.category = 'saving withdrawal')
                                            and t_filter.date > t.date
                                            and t.transactionId = t_filter.transactionId
                                            order by t_filter.date asc )
            and t1.transactionId = t.transactionId) end as "Dátumok",
            *
from dbo.transactions t 
    where type = 'income'

--első verzió
Select 
    case when 
    t.category = 'saving' or t.category = 'saving withdrawal' then t.amount
    else (select sum(t1.amount) from dbo.transactions t1  where t1.type = 'income' and
            (t1.category <> 'saving' or t1.category <> 'saving withdrawal') 
            and t1.date >= t.date
            )end,
    *
from dbo.transactions t 
    where type = 'income'


--második verzió
Select 
    case when 
    t.category = 'saving' or t.category = 'saving withdrawal' then t.amount
    else (select sum(t1.amount) from dbo.transactions t1  where t1.type = 'income' and
            (t1.category <> 'saving' or t1.category <> 'saving withdrawal') 
            and t1.date >= t.date
            and t1.date <= (select top 1 t_filter.date from dbo.transactions t_filter where (t1.category = 'saving' or t1.category = 'saving withdrawal')
            and t_filter.date >= t1.date order by t_filter.date asc)
            )end
from dbo.transactions t 
    where type = 'income'

Select 
   (select sum(t.amount) from dbo.transactions t where t.category='saving' and t.date = s.date)
from dbo.saving s
    
select
    sum(amount)
from dbo.transactions t
    where type = 'income'


with saving_s as (select sum (amount) as s, s.date as d from dbo.saving s group by s.date), transactions_s as ((select sum(amount) as t1, t.date as d from dbo.transactions t where type = 'income' group by t.date)) select case when t.d is null then s.d else t.d end as d from transactions_s t full outer join saving_s s on s.d = t.d  


