#include "DigiKeyboard.h"

void setup() {
  // don't need to set anything up to use DigiKeyboard
}


void loop() {
  // this is generally not necessary but with some older systems it seems to
  // prevent missing the first character after a delay:
  DigiKeyboard.sendKeyStroke(0);
  
  // Type out this string letter by letter on the computer (assumes US-style
  // keyboard)
  
  // WIN+Q
  DigiKeyboard.sendKeyStroke(KEY_Q, MOD_GUI_LEFT);
  DigiKeyboard.delay(200);
  // Enter Powershell
  DigiKeyboard.println("powershell");
  DigiKeyboard.delay(200);
  // ENTER
  DigiKeyboard.sendKeyStroke(KEY_ENTER);
  DigiKeyboard.delay(100000);
}
