# Accessing excel via ADO.NET #

Example:

```
@main[]
	$table[^table::excel[SELECT * FROM [Лист1^$A4:A7];../../resources/sample.xls]]
	some text
	^table.menu{
		<cell>^table.column[0]</cell>
	}
```