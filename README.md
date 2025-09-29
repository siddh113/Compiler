# MiniTriangle Compiler

This is a compiler for a small programming language called MiniTriangle. It's written in C\# and demonstrates the classic stages of a compiler:

1.  **Tokenization**: Breaking down the source code into a stream of tokens.
2.  **Parsing**: Building an abstract syntax tree (AST) from the tokens.
3.  **Semantic Analysis**: Checking the AST for type errors and other semantic issues.
4.  **Code Generation**: Translating the AST into target machine code.

The compiler takes a MiniTriangle source file (`.tri`) and produces two output files: a binary file (`.tam`) for the Triangle Abstract Machine and a textual assembly file (`.txt`).

-----

## Getting Started

### Prerequisites

  * .NET 8.0 SDK

### Building and Running the Compiler

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/siddh113/Compiler.git
    ```
2.  **Navigate to the project directory:**
    ```sh
    cd Compiler/MiniTriangleCompiler/CompleteTriangleCompiler/Compiler
    ```
3.  **Build the project:**
    ```sh
    dotnet build
    ```
4.  **Run the compiler:**
    ```sh
    dotnet run <input_file.tri> <output_file.tam> <output_file.txt>
    ```

-----

## Usage

The compiler is a command-line tool that takes three arguments:

1.  `inputFile`: The path to the MiniTriangle source file (`.tri`).
2.  `binaryOutputFile`: The path to write the binary target code to (`.tam`).
3.  `textOutputFile`: The path to write the text assembly code to (`.txt`).

For example, to compile a file named `example.tri` and create `output.tam` and `output.txt`, you would run:

```sh
dotnet run example.tri output.tam output.txt
```

-----

## MiniTriangle Language

The MiniTriangle language is a simple imperative language with the following features:

  * **Variables and Constants**: Declare variables with `var` and constants with `const`.
  * **Assignments**: Assign values to variables using `:=`.
  * **Control Flow**: `if-then-else`, `while-do`, and `loop-while-repeat` statements.
  * **Expressions**: Integer and character literals, binary and unary operators.
  * **Functions**: Call predefined functions.

### Example MiniTriangle Program (`example.tri`)

```
! This is a comment

let
  const N ~ 10;
  var x : Integer
in
  begin
    x := 0;
    while x < N do
      x := x + 1
  end
```

-----

## Compiler Internals

The compiler is organized into several key components:

  * **Tokenizer (`Tokenizer.cs`)**: The tokenizer reads the source code and converts it into a sequence of tokens. It handles whitespace, comments, keywords, identifiers, literals, and operators.
  * **Parser (`Parser.cs`)**: The parser takes the list of tokens and builds an abstract syntax tree (AST). It uses a recursive descent parsing strategy to recognize the grammatical structure of the language.
  * **Semantic Analyzer (`TypeChecker.cs`)**: The type checker traverses the AST to ensure that the program is semantically correct. This includes checking for type mismatches, undeclared variables, and other errors.
  * **Code Generator (`CodeGenerator.cs`)**: The code generator walks the AST and emits target code for the Triangle Abstract Machine. It manages scopes, runtime entities, and instruction generation.

This structured approach makes the compiler modular and easier to understand. Each phase has a distinct responsibility, and they work together to translate the high-level MiniTriangle language into low-level machine code.
