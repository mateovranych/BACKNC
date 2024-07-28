import { Component, inject, Input } from '@angular/core';
import { Card, IdCard } from '../../models/card.interface';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../material/material.module';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-card-list',
  standalone: true,
  imports: [CommonModule, MaterialModule, RouterModule],
  templateUrl: './card-list.component.html',
  styleUrl: './card-list.component.scss',
})
export class CardListComponent {
  
  @Input()
  professionals: Card[] = [];

}
