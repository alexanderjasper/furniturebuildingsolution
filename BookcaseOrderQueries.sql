select b.Id BookcaseId, b.Created, b.Modified, b.UserId, b.Name as BookcaseName, u.EmailAddress from Bookcases b
join users u on b.UserId = u.id

select OrderNumber, OrderStatus, OrderTime, FirstName, LastName, BookcaseId, Quantity, UnitPrice, b.Name as BookcaseName, a.AddressString, m.Name as Material from Orders o
join OrderItems oi on oi.DbOrderId = o.Id
join Bookcases b on oi.BookcaseId = b.id
join Addresses a on a.Id = o.AddressId
join Materials m on b.MaterialId = m.Id