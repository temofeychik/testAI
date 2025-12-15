import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface Quote {
  id: number;
  text: string;
  author?: string | null;
  language: string;
  likes: number;
  tags: string;
  image?: string | null;
}

export interface QuotePayload {
  text: string;
  author?: string;
  language: string;
  likes: number;
  tags?: string;
  image?: string;
}

@Injectable({ providedIn: 'root' })
export class QuotesService {
  private readonly baseUrl = `${environment.apiUrl}/quotes`;

  constructor(private readonly http: HttpClient) {}

  getQuotes(): Observable<Quote[]> {
    return this.http.get<Quote[]>(this.baseUrl);
  }

  createQuote(payload: QuotePayload): Observable<Quote> {
    return this.http.post<Quote>(this.baseUrl, payload);
  }

  updateQuote(id: number, payload: QuotePayload): Observable<Quote> {
    return this.http.put<Quote>(`${this.baseUrl}/${id}`, payload);
  }

  deleteQuote(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
