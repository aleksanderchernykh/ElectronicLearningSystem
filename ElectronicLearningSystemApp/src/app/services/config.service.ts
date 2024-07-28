import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  readonly API_URL = 'https://api.example.com';

  readonly USER_TYPE = {
    Administrator: "02bc926f-9c56-4fb9-bc8e-68bbe2e87c17",
    Student: "86b8ca0b-85ce-4aca-b911-28836645ebc7",
    Teacher: "c0eb7e9a-b913-4cd0-bf70-146fc48764ba"
  }
}