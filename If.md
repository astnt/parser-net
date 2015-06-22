# If and Else #


Example without 'else':

```
	$something[val]
	^if($something eq 'val'){
		something is equal
	}
```
result is 'something is equal'.


Example with 'else':

```
@main[]
	$somevar[458]
	^if($somevar == 100){
		true
	}{
		false
	}
```
result is 'false' from 'else' branch.