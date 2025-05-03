using Compiler.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Tokenization
{
    /// <summary>
    /// A tokenizer for the MINISQUARE-22 language
    /// </summary>
    public class Tokenizer
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The reader getting the characters from the file
        /// </summary>
        private IFileReader Reader { get; }

        /// <summary>
        /// The characters currently in the token
        /// </summary>
        private StringBuilder TokenSpelling { get; } = new StringBuilder();

        /// <summary>
        /// Creates a new tokenizer
        /// </summary>
        public Tokenizer(IFileReader reader, ErrorReporter reporter)
        {
            Reader = reader;
            Reporter = reporter;
        }

        /// <summary>
        /// Gets all the tokens from the file
        /// </summary>
        public List<Token> GetAllTokens()
        {
            List<Token> tokens = new List<Token>();
            Token token = GetNextToken();
            while (token.Type != TokenType.EndOfText)
            {
                tokens.Add(token);
                token = GetNextToken();
            }
            tokens.Add(token);
            Reader.Close();
            return tokens;
        }

        /// <summary>
        /// Gets the next token
        /// </summary>
        private Token GetNextToken()
        {
            SkipSeparators();
            Position tokenStartPosition = Reader.CurrentPosition;
            TokenType tokenType = ScanToken();
            Token token = new Token(tokenType, TokenSpelling.ToString(), tokenStartPosition);
            Debugger.Write($"Scanned {token}");

            if (tokenType == TokenType.Error)
                Reporter.AddError($"Lexical Error at {tokenStartPosition}: Unrecognized token '{TokenSpelling}'");

            return token;
        }

        /// <summary>
        /// Skips whitespace and comments
        /// </summary>
        private void SkipSeparators()
        {
            while (char.IsWhiteSpace(Reader.Current) || Reader.Current == '/' || Reader.Current == '!')
            {
                if (Reader.Current == '!' || (Reader.Current == '/' && Peek() == '/'))
                    Reader.SkipRestOfLine();
                else if (Reader.Current == '/' && Peek() == '*')
                {
                    Reader.MoveNext();
                    Reader.MoveNext();
                    while (!(Reader.Current == '*' && Peek() == '/') && Reader.Current != '\0')
                        Reader.MoveNext();
                    Reader.MoveNext();
                    Reader.MoveNext();
                }
                else
                    Reader.MoveNext();
            }
        }

        /// <summary>
        /// Reads the next token from the source
        /// </summary>
        private TokenType ScanToken()
        {
            TokenSpelling.Clear();

            if (char.IsLetter(Reader.Current))
            {
                // Identifiers and Keywords
                TakeIt();
                while (char.IsLetterOrDigit(Reader.Current))
                    TakeIt();
                return TokenTypes.IsKeyword(TokenSpelling) ? TokenTypes.GetTokenForKeyword(TokenSpelling) : TokenType.Identifier;
            }
            else if (char.IsDigit(Reader.Current))
            {
                // Integer Literals
                TakeIt();
                while (char.IsDigit(Reader.Current))
                    TakeIt();
                return TokenType.IntLiteral;
            }
            else if (IsOperatorStart(Reader.Current))
            {
                // Operators and multi-character symbols (=>, <=, >=, ==, !=)
                TakeIt();
                if (Reader.Current == '=')
                    TakeIt();
                string op = TokenSpelling.ToString();
                return TokenTypes.SpecialSymbols.ContainsKey(op) ? TokenTypes.SpecialSymbols[op] : TokenType.Operator;
            }
            else if (Reader.Current == ':')
            {
                // Colon or Assignment :=
                TakeIt();
                if (Reader.Current == '=')
                {
                    TakeIt();
                    return TokenType.Becomes;
                }
                return TokenType.Colon;
            }
            else if (Reader.Current == '?')
            {
                // Quick If (?)
                TakeIt();
                return TokenType.QuestionMark;
            }
            else if (Reader.Current == ';')
            {
                // Semicolon (;)
                TakeIt();
                return TokenType.Semicolon;
            }
            else if (Reader.Current == '~')
            {
                // Is (~)
                TakeIt();
                return TokenType.Is;
            }
            else if (Reader.Current == '(')
            {
                // Left Bracket
                TakeIt();
                return TokenType.LeftBracket;
            }
            else if (Reader.Current == ')')
            {
                // Right Bracket
                TakeIt();
                return TokenType.RightBracket;
            }
            else if (Reader.Current == '\'')
            {
                // Character Literal Fix
                return ReadCharacterLiteral();
            }
            else if (Reader.Current == '\0')
            {
                // End of file
                TakeIt();
                return TokenType.EndOfText;
            }
            else
            {
                // Unknown token
                TakeIt();
                Reporter.AddError($"Lexical Error at {Reader.CurrentPosition}: Unrecognized character '{TokenSpelling}'");
                return TokenType.Error;
            }
        }

        /// <summary>
        /// Reads a character literal properly
        /// </summary>
        private TokenType ReadCharacterLiteral()
        {
            TakeIt(); // Consume opening '

            if (Reader.Current == '\n' || Reader.Current == '\0')
            {
                Reporter.AddError($"Lexical Error at {Reader.CurrentPosition}: Unterminated character literal");
                return TokenType.Error;
            }

            char character = Reader.Current;
            TakeIt(); // Consume character inside ''

            if (Reader.Current == '\'')
            {
                TakeIt(); // Consume closing '
                return TokenType.CharLiteral;
            }
            else
            {
                Reporter.AddError($"Lexical Error at {Reader.CurrentPosition}: Invalid character literal '{character}'");
                return TokenType.Error;
            }
        }

        /// <summary>
        /// Appends the current character to the token and moves forward
        /// </summary>
        private void TakeIt()
        {
            TokenSpelling.Append(Reader.Current);
            Reader.MoveNext();
        }

        /// <summary>
        /// Peeks at the next character without consuming it
        /// </summary>
        private char Peek()
        {
            Reader.MoveNext();
            char nextChar = Reader.Current;
            Reader.MoveBack();
            return nextChar;
        }

        /// <summary>
        /// Determines if a character starts an operator
        /// </summary>
        private static bool IsOperatorStart(char c)
        {
            return "+-*/<>=!".Contains(c);
        }
    }
}
