UPDATE dbo.WastedItems
SET 
Price = 
(SELECT TOP(1) prices.Price from
(SELECT w.ItemId as Id, m.Price as Price FROM dbo.WastedItems as w
INNER JOIN dbo.Items as i ON w.ItemId = i.Id 
INNER JOIN dbo.Manufacturers as m ON i.ManufacturerId = m.Id) as prices
WHERE prices.Id = ItemId)

ALTER TABLE dbo.WastedItems
ALTER COLUMN Price
    decimal(18, 2) NOT NULL;