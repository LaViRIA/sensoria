const int cob0g = 3;
const int cob0r = 5;  
const int cob0b = 6;

unsigned char r = 100;
unsigned char g = 10;
unsigned char b = 200;

void setup() {
  pinMode(cob0g, OUTPUT); 
  pinMode(cob0r, OUTPUT); 
  pinMode(cob0b, OUTPUT); 
}
int out = 0;

void loop() {
  testColorCob0 (r, g, b);
  r++;
  g++;
  b++;
  delay(10);
  /*
  analogWrite(analogPin, out);
  out = (out > 255) ? 0 : out + 1;
  delay(10);
  */
}

void testColorCob0 (unsigned char r, unsigned char g, unsigned char b) {
  analogWrite(cob0r, r);
  analogWrite(cob0g, g);
  analogWrite(cob0b, b);
}