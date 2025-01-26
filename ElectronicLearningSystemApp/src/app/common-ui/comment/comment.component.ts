import { Component, Input } from '@angular/core';
import { Comment } from '../../interfaces/comment';
import { CommonModule } from '@angular/common';
import { MiniProfileComponent } from '../mini-profile/mini-profile.component';

@Component({
  selector: 'app-comment',
  standalone: true,
  imports: [CommonModule, MiniProfileComponent],
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.scss'
})
export class CommentComponent {
  @Input() comment!: Comment;
}
