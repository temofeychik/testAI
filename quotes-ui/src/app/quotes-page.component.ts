import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Quote, QuotePayload, QuotesService } from './quotes.service';

@Component({
  selector: 'app-quotes-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './quotes-page.component.html',
  styleUrls: ['./quotes-page.component.css'],
})
export class QuotesPageComponent implements OnInit {
  quotes: Quote[] = [];
  form: FormGroup;
  isEditing = false;
  selectedId: number | null = null;
  status = '';
  isLoading = false;

  constructor(private readonly service: QuotesService, formBuilder: FormBuilder) {
    this.form = formBuilder.group({
      text: ['', [Validators.required, Validators.maxLength(1024)]],
      author: [''],
      language: ['ru', Validators.required],
      likes: [0, [Validators.min(0)]],
      tags: [''],
      image: [''],
    });
  }

  ngOnInit(): void {
    this.loadQuotes();
  }

  loadQuotes(): void {
    this.isLoading = true;
    this.service.getQuotes().subscribe({
      next: (quotes) => {
        this.quotes = quotes;
        this.isLoading = false;
      },
      error: () => {
        this.status = 'Не удалось загрузить цитаты';
        this.isLoading = false;
      },
    });
  }

  startCreate(): void {
    this.isEditing = false;
    this.selectedId = null;
    this.form.reset({ text: '', author: '', language: 'ru', likes: 0, tags: '', image: '' });
  }

  startEdit(quote: Quote): void {
    this.isEditing = true;
    this.selectedId = quote.id;
    this.form.setValue({
      text: quote.text,
      author: quote.author ?? '',
      language: quote.language,
      likes: quote.likes,
      tags: quote.tags,
      image: quote.image ?? '',
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload: QuotePayload = { ...this.form.value } as QuotePayload;

    if (this.isEditing && this.selectedId !== null) {
      this.service.updateQuote(this.selectedId, payload).subscribe({
        next: (quote) => {
          this.quotes = this.quotes.map((q) => (q.id === quote.id ? quote : q));
          this.status = `Цитата #${quote.id} обновлена`;
          this.startCreate();
        },
        error: () => (this.status = 'Ошибка при сохранении изменений'),
      });
    } else {
      this.service.createQuote(payload).subscribe({
        next: (quote) => {
          this.quotes = [...this.quotes, quote];
          this.status = `Добавлена цитата #${quote.id}`;
          this.startCreate();
        },
        error: () => (this.status = 'Ошибка при создании записи'),
      });
    }
  }

  deleteQuote(id: number): void {
    if (!confirm('Удалить выбранную цитату?')) {
      return;
    }

    this.service.deleteQuote(id).subscribe({
      next: () => {
        this.quotes = this.quotes.filter((q) => q.id !== id);
        this.status = `Цитата #${id} удалена`;
        if (this.selectedId === id) {
          this.startCreate();
        }
      },
      error: () => (this.status = 'Не удалось удалить запись'),
    });
  }
}
