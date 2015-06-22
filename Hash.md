# Hash class #

Example of usage:
```
@main[]
	$man[^hash::create[]]

	<p>Name of class is $man</p>

	^man.addKey[name;Вася]
	^man.addKey[age;22]
	^man.addKey[sex;m]

	name=^man.getKey[name]
	age=^man.getKey[age]
	sex=^man.getKey[sex]
```

Output:
```
    <p>Name of class is Parser.Model.Hash</p>


    name=Вася
    age=22
    sex=m
```

## Each (foreach) ##

Example:
```
@main[]
	$man[^hash::create[]]

	^man.addKey[name;Вася]
	^man.addKey[age;22]
	^man.addKey[sex;m]

	^man.each[key;value]{
		$key = $value
	}
```

Output:
```
        name = Вася
        age = 22
        sex = m
```