# Var in parser context #

Add variable named "this" in .cs:
```
ParserFacade pf = new ParserFacade();
pf.Parse(Block.Data);
pf.AddVar("this", this);
string actual = pf.Run();
```

In parser:
```
@main[]
  name of class $this
  call method ^this.method[]
```